using Microsoft.AspNetCore.Mvc;
using WebApplication1.Repositories.WorkSpaces;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data; 
// using WebApplication1.Data; 

namespace WebApplication1.Controllers.Owner
{
    public class WorkSpaceController : Controller
    {
        private readonly IWorkSpaceRepository _workSpaceRepo;
        private readonly ApplicationDbContext _context; // تعريف الـ DbContext

        // حقن الـ Context في الـ Constructor
        public WorkSpaceController(IWorkSpaceRepository workSpaceRepo, ApplicationDbContext context)
        {
            _workSpaceRepo = workSpaceRepo;
            _context = context;
        }

        public IActionResult MangeWorkSpace()
        {
            var ownerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (ownerId == null) return RedirectToAction("Login", "Account");

            var workspace = _workSpaceRepo.GetWorkSpaceByOwnerId(ownerId);
            if (workspace == null)
            {
                ViewBag.Menu = new List<MenuItem>(); 
                return View("MangeWorkSpace", null);
            }

   
            ViewBag.Menu = _context.MenuItems
                                   .Where(m => m.WorkSpaceID == workspace.Id)
                                   .ToList();

            return View("MangeWorkSpace", workspace);
        }
        [HttpPost]
        public IActionResult CheckIn(int resourceId, string? userId) // خلينا userId تقبل Null
        {
            var currentTime = DateTime.Now;
            var resource = _context.Resourses.Include(r => r.Bookings).FirstOrDefault(r => r.Id == resourceId);
            
            if (resource == null) return Json(new { success = false, message = "Resource not found." });

            // فحص إنها مش مشغولة دلوقتي
            bool isOccupiedNow = resource.Bookings.Any(b => 
                b.Status == "Active" && 
                b.StartTime <= currentTime && 
                (b.EndTime == null || b.EndTime > currentTime)
            );

            if (isOccupiedNow) return Json(new { success = false, message = "Resource is already occupied right now." });

            // إنشاء الحجز
            var booking = new Booking 
            {
                ResourceId = resourceId,
                StartTime = currentTime,
                Status = "Active" 
            };
            resource.IsAvailable = false;

            // لو الأونر كتب إيميل، ندور عليه ونربطه.. لو مكتبش، نسجل الحجز من غير يوزر (Guest)
            if (!string.IsNullOrEmpty(userId))
            {
                var appUser = _context.Users.FirstOrDefault(u => u.Email == userId);
                if (appUser != null)
                {
                    booking.UserId = appUser.Id;
                }
                // لو حابب تطلعله إيرور إن الإيميل غلط ممكن تحطها هنا، بس الأفضل نمشيها Guest
            }
            
            _context.Bookings.Add(booking);
            _context.SaveChanges();

            return RedirectToAction("MangeWorkSpace");
        
        }

        // دالة الـ End Session
        [HttpPost]

        public IActionResult EndSession(int resourceId)
        {
            var workspaceClaim = User.FindFirst("WorkspaceId")?.Value;
            if (string.IsNullOrEmpty(workspaceClaim) || !int.TryParse(workspaceClaim, out int currentWorkspaceId))
            {
                return RedirectToAction("AccessDenied", "Account");
            }

            var resource = _context.Resourses
                .Include(r => r.Bookings)
                .FirstOrDefault(r => r.Id == resourceId && r.WorkspaceId == currentWorkspaceId);

            if (resource == null)
            {
                return Json(new { success = false, message = "Resource not found or access denied." });
            }

            var activeBooking = resource.Bookings.FirstOrDefault(b => b.Status == "Active");

            if (activeBooking != null)
            {
                var endTime = DateTime.Now;
                activeBooking.EndTime = endTime;
                activeBooking.Status = "Completed";
                resource.IsAvailable = true;

                double totalHours = (endTime - activeBooking.StartTime).TotalHours;
                if (totalHours < 0.1) totalHours = 1;
                decimal totalHoursCost = (decimal)totalHours * resource.HourlyRate;


                decimal totalOrdersCost = _context.Orders
                    .Where(o => o.BookingId == activeBooking.Id)
                    .SelectMany(o => _context.OrderDetails.Where(od => od.OrderId == o.Id))
                    .Sum(od => (decimal?)od.Quantity * od.PriceAtSale) ?? 0.00m;

                var invoice = new Invoice
                {
                    BookingId = activeBooking.Id,
                    IssueDate = endTime,
                    TotalHoursCost = Math.Round(totalHoursCost, 2),
                    TotalOrdersCost = totalOrdersCost,
                    GrandTotal = Math.Round(totalHoursCost + totalOrdersCost, 2),
                    PaymentMethod = "Cash", 
                    WorkSpaceId = currentWorkspaceId 
                };

                _context.Invoices.Add(invoice);
                _context.SaveChanges();

                return Json(new { success = true, invoiceId = invoice.Id });
            }

            return Json(new { success = false, message = "No active session found for this resource." });
        }

        [HttpGet]
        public IActionResult GetLiveFloor()
        {
            var ownerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (ownerId == null) return BadRequest();

            // بنجيب أحدث داتا من الداتابيز
            var workspace = _workSpaceRepo.GetWorkSpaceByOwnerId(ownerId);

            // بنرجع الـ Partial View مش View كامل
            return PartialView("_LiveFloor", workspace);
        }

        [HttpGet]
        public IActionResult GetResourceBookings(int resourceId)
        {
            var bookings = _context.Bookings
                // .Include(b => b.User) // لو رابط جدول اليوزر وعايز تجيب اسمه، فك الكومنت هنا
                .Where(b => b.ResourceId == resourceId && b.Status != "Cancelled")
                .OrderBy(b => b.StartTime)
                .Select(b => new 
                {
                    bookingId = b.Id,
                    // clientName = b.User != null ? b.User.UserName : "Guest", // لو شغال بجدول اليوزر
                    date = b.StartTime.ToString("yyyy-MM-dd"),
                    startTime = b.StartTime.ToString("HH:mm"),
                    endTime = b.EndTime != null ? b.EndTime.Value.ToString("HH:mm") : "N/A",
                    status = b.Status
                })
                .ToList();

            return Json(bookings);
        }

        [HttpPost]
        public IActionResult ChangeBookingStatus(int bookingId, string newStatus)
        {
            // بندور على الحجز برقم الـ ID بتاعه
            var booking = _context.Bookings.FirstOrDefault(b => b.Id == bookingId);
            
            if (booking == null)
            {
                return RedirectToAction("MangeWorkSpace");
            }

            // بنغير الحالة بناءً على الزرار اللي الأونر داس عليه (Active, Completed, Cancelled)
            booking.Status = newStatus;

            // لو الأونر بيعمل End Session، بنسجل وقت النهاية الفعلي دلوقتي
            if (newStatus == "Completed")
            {
                booking.EndTime = DateTime.Now;
                // هنا زميلك هيقدر يربط دالة توليد الفاتورة (Invoice) في المستقبل
            }

            _context.SaveChanges();
            return RedirectToAction("MangeWorkSpace");
        }

        [HttpGet]
        public IActionResult GetMenu()
        {
            // بنجيب المنيو من الداتا بيز عشان نعرضه في الـ Dropdown
            var menu = _context.MenuItems.Select(m => new 
            { 
                id = m.Id, 
                name = m.Name, 
                price = m.Price 
            }).ToList();

            return Json(menu);
        }

        [HttpGet]
        public IActionResult GetSessionOrdersPartial(int bookingId)
        {
            var details = _context.OrderDetails
                .Include(od => od.MenuItem)
                .Include(od => od.Order)
                .Where(od => od.Order!.BookingId == bookingId)
                .OrderByDescending(od => od.Id)
                .ToList();

            return PartialView("_SessionOrdersList", details);
        }

        [HttpPost]
        public IActionResult AddOrdersToSession(int bookingId, [FromForm] List<int> menuItemId, [FromForm] List<int> quantity)
        {
            var booking = _context.Bookings.FirstOrDefault(b => b.Id == bookingId && b.Status == "Active");
            if (booking == null)
                return Json(new { success = false, message = "Active session not found." });

            if (menuItemId == null || quantity == null || menuItemId.Count == 0)
                return Json(new { success = false, message = "Add at least one item." });

            var order = _context.Orders.FirstOrDefault(o => o.BookingId == bookingId);
            if (order == null)
            {
                order = new Order
                {
                    BookingId = bookingId,
                    OrderDate = DateTime.Now,
                    OrderStatus = "Pending"
                };
                _context.Orders.Add(order);
                _context.SaveChanges();
            }

            var addedCount = 0;
            for (var i = 0; i < menuItemId.Count; i++)
            {
                if (i >= quantity.Count) break;

                var itemId = menuItemId[i];
                var qty = quantity[i];
                if (itemId <= 0 || qty <= 0) continue;

                var menuItem = _context.MenuItems.FirstOrDefault(m => m.Id == itemId);
                if (menuItem == null) continue;

                _context.OrderDetails.Add(new OrderDetails
                {
                    OrderId = order.Id,
                    MenuItemId = itemId,
                    Quantity = qty,
                    PriceAtSale = menuItem.Price
                });
                addedCount++;
            }

            if (addedCount == 0)
                return Json(new { success = false, message = "No valid items were added." });

            _context.SaveChanges();
            return Json(new { success = true, message = $"{addedCount} item(s) added to the bill.", bookingId });
        }
    }
}