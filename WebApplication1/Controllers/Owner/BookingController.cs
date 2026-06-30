using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers.Owner
{
    [Authorize(Roles = "Owner")]
    public class BookingController : Controller
    {
        public IActionResult Bookings()
        {
            var workspaceId = User.FindFirst("WorkspaceId")?.Value;
            return View("Bookings", workspaceId);
        }
    }
}
