using Microsoft.AspNetCore.Identity;
using WebApplication1.Models.Domain.User;
using WebApplication1.Models.ViewModels;

namespace WebApplication1.Repositories.Auth
{
    public interface IAuthRepositorie
    {
        Task<ApplicationUser?> FindByEmailAsync(string email);
        Task<ApplicationUser?> FindByIdAsync(string userId);
        Task<SignInResult> PasswordSignInAsync(string userName, string password, bool isPersistent);
        Task SignInWithClaimsAsync(ApplicationUser user, bool isPersistent, string firstName, string lastName);
        Task<IList<string>> GetUserRolesAsync(ApplicationUser user);
        Task<IdentityResult> CreateUserAsync(ApplicationUser user, string password);
        Task<bool> RoleExistsAsync(string roleName);
        Task<IdentityResult> AddToRoleAsync(ApplicationUser user, string roleName);
        Task SignOutAsync(); 
        Task<bool> CreateWorkspaceAsync(WorkspaceSetupViewModel model, string ownerId);
    }
}
