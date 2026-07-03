using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Data;

namespace WebApplication1.Repositories.Menu
{
    public class MenuItemRepository : IMenuItemRepository
    {
        private readonly ApplicationDbContext _context;

        public MenuItemRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<MenuItem>> GetByWorkspaceIdAsync(int workspaceId)
        {
            return await _context.MenuItems
                .Where(m => m.WorkSpaceID == workspaceId)
                .ToListAsync();
        }

        public async Task<bool> HasMenuAsync(int workspaceId)
        {
            return await _context.MenuItems.AnyAsync(m => m.WorkSpaceID == workspaceId);
        }

        public async Task AddMenuItemsAsync(List<MenuItem> items)
        {
            await _context.MenuItems.AddRangeAsync(items);
            await _context.SaveChangesAsync();
        }

        public async Task<MenuItem> GetByIdAsync(int id)
        {
            return await _context.MenuItems.FindAsync(id);
        }

        public async Task UpdateAsync(MenuItem item)
        {
            _context.MenuItems.Update(item);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var item = await GetByIdAsync(id);
            if (item != null)
            {
                _context.MenuItems.Remove(item);
                await _context.SaveChangesAsync();
            }
        }
    }
}