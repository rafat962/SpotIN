using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebApplication1.Models.ViewModel.Profile;
using WebApplication1.Repositories.Auth;
using WebApplication1.Repositories.Profile;

namespace WebApplication1.Controllers.Owner
{
    [Authorize(Roles = "Owner")]
    public class ProfileController : Controller
    {
        private readonly IOwnerProfileRepository _profileRepository;
        private readonly IAuthRepositorie _authRepository;

        public ProfileController(IOwnerProfileRepository profileRepository, IAuthRepositorie authRepository)
        {
            _profileRepository = profileRepository;
            _authRepository = authRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var ownerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (ownerId == null) return RedirectToAction("Login", "Account");

            var model = await _profileRepository.GetProfileByOwnerIdAsync(ownerId);
            if (model == null)
            {
                return RedirectToAction("Index", "Owner");
            }

            ViewData["Title"] = "Profile";
            ViewData["PageTitle"] = "Owner Profile";
            ViewData["Breadcrumb"] = "SpotIN · Owner Portal · Profile";

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(OwnerProfileViewModel model)
        {
            var ownerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (ownerId == null) return RedirectToAction("Login", "Account");

            if (model.UserId != ownerId)
            {
                return Forbid();
            }

            ViewData["Title"] = "Profile";
            ViewData["PageTitle"] = "Owner Profile";
            ViewData["Breadcrumb"] = "SpotIN · Owner Portal · Profile";

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var updated = await _profileRepository.UpdateProfileAsync(model);
            if (!updated)
            {
                ModelState.AddModelError(string.Empty, "Unable to save profile changes. Please try again.");
                return View(model);
            }

            var user = await _authRepository.FindByIdAsync(ownerId);
            if (user != null)
            {
                var workspaceId = User.FindFirst("WorkspaceId")?.Value ?? string.Empty;
                await _authRepository.SignInWithClaimsAsync(user, isPersistent: false, model.FirstName, model.LastName, workspaceId);
            }

            TempData["SuccessMessage"] = "Profile updated successfully.";
            return RedirectToAction(nameof(Index));
        }
    }
}
