using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models.Domain.User;
using WebApplication1.Models.ViewModel.Account;
using WebApplication1.Models.ViewModels;
using WebApplication1.Repositories;
using WebApplication1.Repositories.Auth;

namespace WebApplication1.Controllers.Auth
{
    public class AccountController : Controller
    {
        private readonly IAuthRepositorie _authRepo;

        // حقن الـ Repository فقط هنا
        public AccountController(IAuthRepositorie authRepo)
        {
            _authRepo = authRepo;
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

            var user = await _authRepo.FindByEmailAsync(model.Email);
            if (user == null)
            {
                ViewBag.ErrorMessage = "Invalid email or password.";
                return View("Login", model);
            }

            var result = await _authRepo.PasswordSignInAsync(user.UserName, model.Password, model.RememberMe);

            if (result.Succeeded)
            {
                await _authRepo.SignInWithClaimsAsync(user, model.RememberMe, user.FirstName, user.LastName);

                var roles = await _authRepo.GetUserRolesAsync(user);
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

        [HttpGet]
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

            var userExists = await _authRepo.FindByEmailAsync(model.Email);
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

            var result = await _authRepo.CreateUserAsync(user, model.Password);

            if (result.Succeeded)
            {
                string assignedRole = model.Role == "Owner" ? "Owner" : "Client";

                if (await _authRepo.RoleExistsAsync(assignedRole))
                {
                    await _authRepo.AddToRoleAsync(user, assignedRole);
                }

                if (assignedRole == "Client")
                {
                    await _authRepo.SignInWithClaimsAsync(user, isPersistent: true, user.FirstName, user.LastName);
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
            await _authRepo.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }

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
            // 1. استرجاع البيانات الحالية من الـ TempData
            var userId = TempData["PendingOwnerId"]?.ToString();
            var firstName = TempData["FirstName"]?.ToString() ?? "";
            var lastName = TempData["LastName"]?.ToString() ?? "";

            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("SignUp");
            }

            if (!ModelState.IsValid)
            {
                TempData.Keep("PendingOwnerId");
                TempData.Keep("FirstName");
                TempData.Keep("LastName");
                return View("SetupWorkspace", model);
            }

            var user = await _authRepo.FindByIdAsync(userId);
            if (user == null)
            {
                return RedirectToAction("SignUp");
            }

            var isSaved = await _authRepo.CreateWorkspaceAsync(model, userId);

            if (!isSaved)
            {
                ModelState.AddModelError(string.Empty, "Something went wrong while creating the workspace. Please try again.");
                TempData.Keep("PendingOwnerId");
                TempData.Keep("FirstName");
                TempData.Keep("LastName");
                return View("SetupWorkspace", model);
            }

            await _authRepo.SignInWithClaimsAsync(user, isPersistent: true, firstName, lastName);

            return RedirectToAction("Index", "Owner");
        }
    }
}