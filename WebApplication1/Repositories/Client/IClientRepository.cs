using WebApplication1.Models.Domain.User;

namespace WebApplication1.Repositories.Client
{
    public interface IClientRepository
    {
        // Explore
        Task<List<WebApplication1.Models.Domain.WorkSpaces.WorkSpace>> GetAllWorkspacesAsync();

        // Profile
        Task<ApplicationUser?> GetUserWithBookingsAsync(string userId);

        // Request
        Task<List<WebApplication1.Models.Domain.WorkSpaces.WorkSpace>> GetAvailableWorkspacesAsync();
        Task<Resourse?> GetResourceByIdAsync(int resourceId);
        Task<bool> CreateBookingAsync(string userId, int resourceId, DateTime startTime);
    }
}
