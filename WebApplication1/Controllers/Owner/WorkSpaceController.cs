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
            
            if (ownerId == null)
            {
                return RedirectToAction("Login", "Account"); 
            }

            var workspace = _workSpaceRepo.GetWorkSpaceByOwnerId(ownerId);

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
            
            return Json(new { success = true });
        }

        // دالة الـ End Session
        [HttpPost]
        public IActionResult EndSession(int resourceId)
        {
            // بنجيب الترابيزة ومعاها كل الحجوزات اللي حالتها Active
            var resource = _context.Resourses
                .Include(r => r.Bookings.Where(b => b.Status == "Active"))
                .FirstOrDefault(r => r.Id == resourceId);
            
            if (resource == null)
            {
                return Json(new { success = false, message = "Resource not found." });
            }

            // ندور على أي حجز "Active" للترابيزة دي
            var activeBooking = resource.Bookings.FirstOrDefault();
            
            if (activeBooking != null)
            {
                activeBooking.EndTime = DateTime.Now;
                activeBooking.Status = "Completed"; 
                
                _context.SaveChanges();
                return Json(new { success = true });
            }
            else
            {
                // لو مفيش حجز active بس الترابيزة لسة حمراء، يبقى في داتا قديمة معلقة
                // هنسمح للأونر ينهي الجلسة حتى لو الحجز مش موجود عشان يفك التعليق
                return Json(new { success = true }); 
            }
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
                return Json(new { success = false, message = "Booking not found." });
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
            return Json(new { success = true });
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

        [HttpPost]
        public IActionResult AddOrderToSession(int bookingId, int menuItemId, int quantity)
        {
            // 1. بنتأكد إن الحجز موجود
            var booking = _context.Bookings.FirstOrDefault(b => b.Id == bookingId);
            if (booking == null) return Json(new { success = false, message = "Session not found." });

            // 2. بنجيب المنتج من المنيو عشان نعرف سعره
            var menuItem = _context.MenuItems.FirstOrDefault(m => m.Id == menuItemId);
            if (menuItem == null) return Json(new { success = false, message = "Item not found." });

            // 3. بنشوف هل الحجز ده ليه فاتورة/أوردر مفتوح أصلاً ولا لأ؟
            var order = _context.Orders.FirstOrDefault(o => o.BookingId == bookingId);
            
            // لو ملوش أوردر، بنكريتله واحد جديد
            if (order == null)
            {
                order = new Order 
                { 
                    BookingId = bookingId, 
                    OrderDate = DateTime.Now,
                    OrderStatus = "Pending" 
                };
                _context.Orders.Add(order);
                _context.SaveChanges(); // بنسيف عشان ناخد الـ ID بتاع الأوردر
            }

            // 4. بنضيف المنتج (OrderDetail) جوه الأوردر ده
            var orderDetail = new OrderDetails 
            {
                OrderId = order.Id,
                MenuItemId = menuItemId,
                Quantity = quantity,
                // التعديل هنا: استخدمنا PriceAtSale زي ما موجود في الموديل بتاعك
                PriceAtSale = menuItem.Price * quantity 
            };
            
            _context.OrderDetails.Add(orderDetail);
            _context.SaveChanges();

            return Json(new { success = true });
        }
    }
}