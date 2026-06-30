using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models.Domain.User;
using WebApplication1.Repositories.Client;

namespace WebApplication1.Repositories
{
    public class ClientRepository : IClientRepo
    {
        private readonly ApplicationDbContext _db;

        public ClientRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<List<WorkSpace>> GetAllWorkspacesAsync()
        {
            return await _db.WorkSpaces
                .Include(w => w.Resourses)
                .Include(w => w.Owner)
                .ToListAsync();
        }

        public async Task<ApplicationUser?> GetUserWithBookingsAsync(string userId)
        {
            return await _db.Users
                .Include(u => u.Bookings)
                    .ThenInclude(b => b.Resourse)
                        .ThenInclude(r => r.WorkSpace)
                .Include(u => u.Bookings)
                    .ThenInclude(b => b.Invoices)
                .FirstOrDefaultAsync(u => u.Id == userId);
        }

        public async Task<List<WorkSpace>> GetAvailableWorkspacesAsync()
        {
            return await _db.WorkSpaces
                .Include(w => w.Resourses.Where(r => r.IsAvailable))
                .Include(w => w.Owner)
                .ToListAsync();
        }

        public async Task<Resourse?> GetResourceByIdAsync(int resourceId)
        {
            return await _db.Resourses.FindAsync(resourceId);
        }

        public async Task<bool> CreateBookingAsync(string userId, int resourceId, DateTime startTime)
        {
            var resource = await _db.Resourses.FindAsync(resourceId);

            if (resource == null || !resource.IsAvailable)
                return false;

            var booking = new Booking
            {
                UserId = userId,
                ResourceId = resourceId,
                StartTime = startTime,
                Status = "Active"
            };

            resource.IsAvailable = false;

            _db.Bookings.Add(booking);
            var rows = await _db.SaveChangesAsync();
            return rows > 0;
        }
    }
}
