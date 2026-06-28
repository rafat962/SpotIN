using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models.Domain.User;
using WebApplication1.Models.ViewModel.Account;
using WebApplication1.Models.ViewModels;

namespace WebApplication1.Controllers.Auth
{
    public class AccountController : Controller
    {


        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }


        [HttpGet]
        public IActionResult Login()
        {
            return View("Login");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LoginForm(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var firstError = ModelState.Values.SelectMany(v => v.Errors).FirstOrDefault()?.ErrorMessage;
                ViewBag.ErrorMessage = firstError ?? "Please correct the errors and try again.";
                return View("Login", model);
            }

            // 1. البحث عن المستخدم بواسطة الإيميل
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                ViewBag.ErrorMessage = "Invalid email or password.";
                return View("Login", model);
            }

            var result = await _signInManager.PasswordSignInAsync(user.UserName, model.Password, model.RememberMe, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                var customClaims = new[] {
                    new System.Security.Claims.Claim("FirstName", user.FirstName ?? ""),
                    new System.Security.Claims.Claim("LastName", user.LastName ?? "")
                };

                await _signInManager.SignInWithClaimsAsync(user, isPersistent: model.RememberMe, customClaims);

                var roles = await _userManager.GetRolesAsync(user);
                if (roles.Contains("Owner"))
                {
                    return RedirectToAction("Index", "Owner");
                }
                else if (roles.Contains("Client"))
                {
                    return RedirectToAction("Index", "Client");
                }

                return RedirectToAction("Login", "Account");
            }

            ViewBag.ErrorMessage = "Invalid email or password.";
            return View("Login", model);
        }


        public IActionResult SignUp()
        {
            return View("SignUp"); 
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (model.Role == "Owner" && string.IsNullOrWhiteSpace(model.Organization))
            {
                ModelState.AddModelError("Organization", "Organization name is required for Workspace Owners.");
            }

            if (!ModelState.IsValid)
            {
                var firstError = ModelState.Values.SelectMany(v => v.Errors).FirstOrDefault()?.ErrorMessage;
                ViewBag.ErrorMessage = firstError ?? "Please correct the errors and try again.";
                return View("SignUp", model);
            }

            var userExists = await _userManager.FindByEmailAsync(model.Email);
            if (userExists != null)
            {
                ViewBag.ErrorMessage = "This email address is already registered.";
                return View("SignUp", model);
            }

            var user = new ApplicationUser
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                UserName = model.Email,
                Email = model.Email
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                string assignedRole = model.Role == "Owner" ? "Owner" : "Client";

                if (await _roleManager.RoleExistsAsync(assignedRole))
                {
                    await _userManager.AddToRoleAsync(user, assignedRole);
                }

                if (assignedRole == "Client")
                {
                    var customClaims = new[] {
                        new System.Security.Claims.Claim("FirstName", user.FirstName),
                        new System.Security.Claims.Claim("LastName", user.LastName)
                    };
                    await _signInManager.SignInWithClaimsAsync(user, isPersistent: true, customClaims);
                    return RedirectToAction("Index", "Client");
                }

                TempData["PendingOwnerId"] = user.Id;
                TempData["FirstName"] = user.FirstName;
                TempData["LastName"] = user.LastName;

                return RedirectToAction("SetupWorkspace", "Account");
            }

            var identityError = result.Errors.FirstOrDefault()?.Description;
            ViewBag.ErrorMessage = identityError ?? "Registration failed. Please try again.";
            return View("SignUp", model);
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction("Login", "Account");
        }



        // 1. عرض صفحة الـ Workspace Setup بالـ Layout الأساسي (GET)
        [HttpGet]
        public IActionResult SetupWorkspace()
        {
            if (TempData["PendingOwnerId"] == null)
            {
                return RedirectToAction("SignUp");
            }

            TempData.Keep("PendingOwnerId");
            TempData.Keep("FirstName");
            TempData.Keep("LastName");

            return View("SetupWorkspace", new WorkspaceSetupViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveWorkspace(WorkspaceSetupViewModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData.Keep("PendingOwnerId");
                TempData.Keep("FirstName");
                TempData.Keep("LastName");
                return View("SetupWorkspace", model);
            }

            var userId = TempData["PendingOwnerId"]?.ToString();
            var firstName = TempData["FirstName"]?.ToString() ?? "";
            var lastName = TempData["LastName"]?.ToString() ?? "";

            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("SignUp");
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                // 2. TODO: هنا هتعمل الحفظ في جدول الـ Workspaces وتمرر الـ userId كـ Foreign Key
                // var workspace = new Workspace { Name = model.Name, UserId = userId, ... };
                // await _context.Workspaces.AddAsync(workspace);
                // await _context.SaveChangesAsync();
            }

            var customClaims = new[] {
                new System.Security.Claims.Claim("FirstName", firstName),
                new System.Security.Claims.Claim("LastName", lastName)
            };

            await _signInManager.SignInWithClaimsAsync(user, isPersistent: true, customClaims);

            return RedirectToAction("Index", "Owner");
        }






    }
}
