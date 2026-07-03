using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models.Domain.User;
using WebApplication1.Models.ViewModel.Account;
using WebApplication1.Models.ViewModels;
using WebApplication1.Repositories.Auth;
using WebApplication1.Models.Domain.WorkSpaces;

namespace WebApplication1.Repositories
{
    public class AuthRepository : IAuthRepositorie
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext dbContext;

        public AuthRepository(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<IdentityRole> roleManager,
            ApplicationDbContext dbContext
            
            )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            this.dbContext = dbContext;
        }

        public async Task<ApplicationUser?> FindByEmailAsync(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }

        public async Task<ApplicationUser?> FindByIdAsync(string userId)
        {
            return await _userManager.FindByIdAsync(userId);
        }

        public async Task<SignInResult> PasswordSignInAsync(string userName, string password, bool isPersistent)
        {
            return await _signInManager.PasswordSignInAsync(userName, password, isPersistent, lockoutOnFailure: false);
        }

        public async Task SignInWithClaimsAsync(ApplicationUser user, bool isPersistent, string firstName, string lastName, string workspaceId)
        {
            var customClaims = new[] {
                new Claim("FirstName", firstName ?? ""),
                new Claim("LastName", lastName ?? ""),
                new Claim("WorkspaceId", workspaceId ?? "")
            };
            await _signInManager.SignInWithClaimsAsync(user, isPersistent, customClaims);
        }

        public async Task<IList<string>> GetUserRolesAsync(ApplicationUser user)
        {
            return await _userManager.GetRolesAsync(user);
        }

        public async Task<IdentityResult> CreateUserAsync(ApplicationUser user, string password)
        {
            return await _userManager.CreateAsync(user, password);
        }

        public async Task<bool> RoleExistsAsync(string roleName)
        {
            return await _roleManager.RoleExistsAsync(roleName);
        }

        public async Task<IdentityResult> AddToRoleAsync(ApplicationUser user, string roleName)
        {
            return await _userManager.AddToRoleAsync(user, roleName);
        }

        public async Task SignOutAsync()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<int?> CreateWorkspaceAsync(WorkspaceSetupViewModel model, string ownerId)
        {
            var workspace = new WebApplication1.Models.Domain.WorkSpaces.WorkSpace
            {
                Name = model.Name,
                Address = model.Address,
                Latitude = model.Latitude,
                Longitude = model.Longitude,
                TotalTables = model.TotalTables,
                TotalRooms = model.TotalRooms,
                HasAirConditioning = model.HasAirConditioning,
                HasWiFi = model.HasWiFi,
                HasDrinksAndCafeteria = model.HasDrinksAndCafeteria,
                Description = model.Description,
                OwnerId = ownerId
            };

            for (int i = 1; i <= model.TotalTables; i++) 
            {
                workspace.Resourses.Add(new Resourse { Name = $"T{i}", Type = "Table", IsAvailable = true,HourlyRate = model.TablePricePerHour });
            }

            for (int i = 1; i <= model.TotalRooms; i++)
            {
                workspace.Resourses.Add(new Resourse { Name = $"R{i}", Type = "Room", IsAvailable = true,HourlyRate = model.RoomPricePerHour });
            }

            await dbContext.WorkSpaces.AddAsync(workspace);
            var rowsAffected = await dbContext.SaveChangesAsync();

            return rowsAffected > 0 ? workspace.Id : null;
        }

        public async Task<string> GetWorkspaceIdByOwnerIdAsync(string ownerId)
        {
            var workspace = await dbContext.WorkSpaces.FirstOrDefaultAsync(w => w.OwnerId == ownerId);
            return workspace?.Id.ToString() ?? "";
        }
    }
}