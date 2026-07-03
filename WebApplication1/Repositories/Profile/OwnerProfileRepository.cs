using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models.Domain.User;
using WebApplication1.Models.ViewModel.Profile;

namespace WebApplication1.Repositories.Profile
{
    public class OwnerProfileRepository : IOwnerProfileRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public OwnerProfileRepository(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<OwnerProfileViewModel?> GetProfileByOwnerIdAsync(string ownerId)
        {
            var user = await _userManager.FindByIdAsync(ownerId);
            if (user == null) return null;

            var workspace = await _context.WorkSpaces
                .Include(w => w.Resourses)
                .FirstOrDefaultAsync(w => w.OwnerId == ownerId);

            if (workspace == null) return null;

            return new OwnerProfileViewModel
            {
                UserId = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email ?? string.Empty,
                WorkspaceId = workspace.Id,
                WorkspaceName = workspace.Name,
                Address = workspace.Address,
                Latitude = workspace.Latitude,
                Longitude = workspace.Longitude,
                Description = workspace.Description,
                HasWiFi = workspace.HasWiFi,
                HasAirConditioning = workspace.HasAirConditioning,
                HasDrinksAndCafeteria = workspace.HasDrinksAndCafeteria,
                TotalTables = workspace.TotalTables,
                TotalRooms = workspace.TotalRooms,
                Resources = workspace.Resourses
                    .OrderBy(r => r.Type)
                    .ThenBy(r => r.Name)
                    .Select(r => new ResourcePricingViewModel
                    {
                        Id = r.Id,
                        Name = r.Name,
                        Type = r.Type,
                        HourlyRate = r.HourlyRate,
                        IsAvailable = r.IsAvailable
                    })
                    .ToList()
            };
        }

        public async Task<bool> UpdateProfileAsync(OwnerProfileViewModel model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId);
            if (user == null) return false;

            var workspace = await _context.WorkSpaces
                .Include(w => w.Resourses)
                .FirstOrDefaultAsync(w => w.Id == model.WorkspaceId && w.OwnerId == model.UserId);

            if (workspace == null) return false;

            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            var userResult = await _userManager.UpdateAsync(user);
            if (!userResult.Succeeded) return false;

            workspace.Name = model.WorkspaceName;
            workspace.Address = model.Address;
            workspace.Latitude = model.Latitude;
            workspace.Longitude = model.Longitude;
            workspace.Description = model.Description;
            workspace.HasWiFi = model.HasWiFi;
            workspace.HasAirConditioning = model.HasAirConditioning;
            workspace.HasDrinksAndCafeteria = model.HasDrinksAndCafeteria;
            workspace.TotalTables = model.TotalTables;
            workspace.TotalRooms = model.TotalRooms;

            if (model.Resources != null)
            {
                foreach (var resourceModel in model.Resources)
                {
                    var resource = workspace.Resourses.FirstOrDefault(r => r.Id == resourceModel.Id);
                    if (resource == null) continue;

                    resource.Name = resourceModel.Name;
                    resource.HourlyRate = resourceModel.HourlyRate;
                }
            }

            await _context.SaveChangesAsync();
            return true;
        }
    }
}
