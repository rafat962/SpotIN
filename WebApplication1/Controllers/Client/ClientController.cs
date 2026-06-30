using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebApplication1.Repositories.Client;

namespace WebApplication1.Controllers.Client
{
    [Authorize(Roles = "Client")]
    public class ClientController : Controller
    {
        private readonly IClientRepository _clientRepo;

        public ClientController(IClientRepository clientRepo)
        {
            _clientRepo = clientRepo;
        }

        // ── Landing Page: Explore Workspaces + Map ──
        public async Task<IActionResult> Index()
        {
            ViewData["PageTitle"] = "Explore Workspaces";
            ViewData["Breadcrumb"] = "SpotIN · Explore";

            var workspaces = await _clientRepo.GetAllWorkspacesAsync();
            return View("Index", workspaces);
        }

        // ── User Profile ──
        public async Task<IActionResult> Profile()
        {
            ViewData["PageTitle"] = "My Profile";
            ViewData["Breadcrumb"] = "SpotIN · Profile";

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _clientRepo.GetUserWithBookingsAsync(userId!);
            return View("Profile", user);
        }

        // ── User Request: Book a Resource ──
        [HttpGet]
        public async Task<IActionResult> Request(int? resourceId)
        {
            ViewData["PageTitle"] = "Book a Spot";
            ViewData["Breadcrumb"] = "SpotIN · New Booking";

            var workspaces = await _clientRepo.GetAvailableWorkspacesAsync();
            ViewBag.SelectedResourceId = resourceId;
            return View("Request", workspaces);
        }

        // ── Submit Booking ──
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubmitBooking(int resourceId, DateTime startTime)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var success = await _clientRepo.CreateBookingAsync(userId!, resourceId, startTime);

            if (!success)
            {
                TempData["Error"] = "This resource is no longer available.";
                return RedirectToAction("Request");
            }

            TempData["Success"] = "Your booking was confirmed!";
            return RedirectToAction("Profile");
        }
    }
}
