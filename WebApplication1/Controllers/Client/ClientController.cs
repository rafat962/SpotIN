using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebApplication1.Repositories.Client;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
// using WebApplication1.Data; // فك الكومنت لو الـ ApplicationDbContext موجود في فولدر Data

namespace WebApplication1.Controllers.Client
{
    [Authorize(Roles = "Client")]
    public class ClientController : Controller
    {
        private readonly IClientRepository _clientRepo;
        private readonly ApplicationDbContext _context; // ضفنا الداتا بيز عشان خوارزمية التعارض

        public ClientController(IClientRepository clientRepo, ApplicationDbContext context)
        {
            _clientRepo = clientRepo;
            _context = context;
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
        // غيرنا الاسم لـ BookSpot عشان نحل الـ Error (CS0108)
        public async Task<IActionResult> BookSpot(int? resourceId) 
        {
            ViewData["PageTitle"] = "Book a Spot";
            ViewData["Breadcrumb"] = "SpotIN · New Booking";

            var workspaces = await _clientRepo.GetAvailableWorkspacesAsync();
            ViewBag.SelectedResourceId = resourceId;
            
            // هنفضل نرجع نفس ملف الـ View اللي اسمه Request.cshtml
            return View("Request", workspaces); 
        }

        // ── Submit Booking ──
        [HttpPost]
        [ValidateAntiForgeryToken]
        // غيرنا البارامترز عشان تستقبل الداتا المتفصصة من الـ HTML
        public async Task<IActionResult> SubmitBooking(int resourceId, string bookingDate, string startTime, string endTime)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // 1. دمج التاريخ مع الوقت عشان نكون متغيرات DateTime صحيحة
            if (!DateTime.TryParse($"{bookingDate} {startTime}", out DateTime parsedStartTime) ||
                !DateTime.TryParse($"{bookingDate} {endTime}", out DateTime parsedEndTime))
            {
                TempData["Error"] = "Invalid date or time format.";
                return RedirectToAction("BookSpot", new { resourceId = resourceId });
            }

            // 2. حائط الصد التاني: خوارزمية فحص التعارض بالتواريخ الدقيقة
            bool isConflict = _context.Bookings.Any(b => 
                b.ResourceId == resourceId && 
                b.Status != "Cancelled" && 
                parsedStartTime < b.EndTime && 
                parsedEndTime > b.StartTime
            );

            if (isConflict)
            {
                TempData["Error"] = "This resource is already booked during the selected time. Please choose another time.";
                return RedirectToAction("BookSpot", new { resourceId = resourceId });
            }

            // 3. الحفظ المباشر في قاعدة البيانات لضمان حفظ كل البيانات
            var booking = new Booking 
            {
                ResourceId = resourceId,
                UserId = userId,
                StartTime = parsedStartTime,
                EndTime = parsedEndTime,
                Status = "Pending" // الحجز المستقبلي بيكون Pending لحد ما الأونر يتدخل
            };

            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Your booking was confirmed!";
            return RedirectToAction("Profile");
        }

        [HttpGet]
        public IActionResult GetBookedTimes(int resourceId, string date)
        {
            // بنتأكد إن التاريخ مبعوث بصيغة صحيحة
            if (!DateTime.TryParse(date, out DateTime selectedDate))
            {
                return Json(new List<object>()); // نرجع لستة فاضية لو التاريخ غلط
            }

            // بنحدد بداية ونهاية اليوم اللي العميل اختاره
            var startOfDay = selectedDate.Date;
            var endOfDay = startOfDay.AddDays(1);

            // بنجيب الحجوزات اللي بتتقاطع مع اليوم ده (ومش ملغية)
            var bookedSlots = _context.Bookings
                .Where(b => b.ResourceId == resourceId && 
                            b.Status != "Cancelled" &&
                            b.StartTime < endOfDay && 
                            b.EndTime > startOfDay)
                .Select(b => new 
                {
                    // بنحول الأوقات لنص عشان الجافاسكريبت يفهمها بسهولة
                    start = b.StartTime.ToString("HH:mm"), 
                    end = b.EndTime.HasValue ? b.EndTime.Value.ToString("HH:mm") : "23:59"
                })
                .ToList();

            return Json(bookedSlots);
        }
    }
}