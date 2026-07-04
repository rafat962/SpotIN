using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using WebApplication1.Repositories.Bookings;

namespace WebApplication1.Controllers.Owner
{
    [Authorize(Roles = "Owner")]
    public class BookingController : Controller
    {
        private readonly IBookingRepo _bookingRepo;
        public BookingController(IBookingRepo bookingRepo)
        {
            _bookingRepo = bookingRepo;
        }

        public async Task<IActionResult> Bookings(string searchString, string BookingStatus)
        {
            var bookings = await _bookingRepo.GetIncomingBookingAsync();
            if (!string.IsNullOrEmpty(searchString))
            {
                bookings = bookings.Where(b => b.User?.UserName != null &&
                                               b.User.UserName.Contains(searchString, StringComparison.OrdinalIgnoreCase));
            }
            if (!string.IsNullOrEmpty(BookingStatus))
            {
                bookings = bookings.Where(b => b.Status == BookingStatus);
            }

            ViewData["CurrentSearch"] = searchString;
            ViewData["CurrentStatus"] = BookingStatus;
            return View(bookings);
        }
        [HttpPost]
        public async Task<IActionResult> AcceptBooking(int id)
        {
            var result = await _bookingRepo.UpdateBookingStatusAsync(id, "Completed");
            if (result)
            {
                return Json(new { success = true, message = "Booking has been accepted successfuly." });
            }
            return Json(new { success = false, message = "Faild to accept the booking." });
        }
        [HttpPost]
        public async Task<IActionResult> RejectBooking(int id)
        {
            var result = await _bookingRepo.UpdateBookingStatusAsync(id, "Cancelled");
            if (result)
            {
                return Json(new { success = true, message = "Booking has been rejected and resource freed." });
            }
            return Json(new { success = false, message = "Faild to reject the booking." });
        }
    }
}
