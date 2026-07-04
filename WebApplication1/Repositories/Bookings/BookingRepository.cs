using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;

namespace WebApplication1.Repositories.Bookings
{
    public class BookingRepository : IBookingRepo
    {
        private readonly ApplicationDbContext _db;
        public BookingRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<Booking> GetBookingByIdAsync(int id)
        {
            return await _db.Bookings.Include(b => b.User)
                .Include(b => b.Resourse)
                .FirstOrDefaultAsync(b => b.Id == id);
            
        }

        public async Task<IEnumerable<Booking>> GetIncomingBookingAsync()
        {
            return await _db.Bookings.Include(b => b.User).Include(b => b.Resourse)
                .ThenInclude(r => r.WorkSpace)
                .OrderByDescending(b => b.StartTime).ToListAsync();
        }

        public async Task<bool> UpdateBookingStatusAsync(int bookingId, string status)
        {
            Booking booking = await GetBookingByIdAsync(bookingId);
            if(booking==null)
            {
                return false;
            }
            booking.Status = status;
            if(status=="Cancelled" && booking.Resourse != null)
            {
                booking.Resourse.IsAvailable = true;
            }
            _db.Bookings.Update(booking);
            return await _db.SaveChangesAsync() > 0;
            
        }
    }
}
