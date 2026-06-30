using WebApplication1.Models.Domain.User;

namespace WebApplication1.Repositories.Client
{
    public interface IClientRepository
    {
        // Explore
        Task<List<WorkSpace>> GetAllWorkspacesAsync();

        // Profile
        Task<ApplicationUser?> GetUserWithBookingsAsync(string userId);

        // Request
        Task<List<WorkSpace>> GetAvailableWorkspacesAsync();
        Task<Resourse?> GetResourceByIdAsync(int resourceId);
        Task<bool> CreateBookingAsync(string userId, int resourceId, DateTime startTime);
    }
}
