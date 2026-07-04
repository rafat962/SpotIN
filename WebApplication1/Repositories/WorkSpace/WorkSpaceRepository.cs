using Microsoft.EntityFrameworkCore;
using System.Linq;
using WebApplication1.Data;
using WebApplication1.Models.Domain.WorkSpaces;

namespace WebApplication1.Repositories.WorkSpaces
{
    public class WorkSpaceRepository : IWorkSpaceRepository
    {
        private readonly ApplicationDbContext _context;

        public WorkSpaceRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public WorkSpace? GetWorkSpaceByOwnerId(string ownerId)
        {
            return _context.WorkSpaces
               .Include(w => w.Resourses)
                   .ThenInclude(r => r.Bookings)
                       .ThenInclude(b => b.Orders)
                           .ThenInclude(o => o.OrderDetails)
                               .ThenInclude(od => od.MenuItem)
               .FirstOrDefault(w => w.OwnerId == ownerId);
        }
    }
}