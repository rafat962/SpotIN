using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApplication1.Repositories.Menu
{
    public interface IMenuItemRepository
    {
        Task<IEnumerable<MenuItem>> GetByWorkspaceIdAsync(int workspaceId);
        Task<bool> HasMenuAsync(int workspaceId);
        Task AddMenuItemsAsync(List<MenuItem> items);
        Task<MenuItem> GetByIdAsync(int id);
        Task UpdateAsync(MenuItem item);
        Task DeleteAsync(int id);
    }
}