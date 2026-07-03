using WebApplication1.Models.ViewModel.Profile;

namespace WebApplication1.Repositories.Profile
{
    public interface IOwnerProfileRepository
    {
        Task<OwnerProfileViewModel?> GetProfileByOwnerIdAsync(string ownerId);
        Task<bool> UpdateProfileAsync(OwnerProfileViewModel model);
    }
}
