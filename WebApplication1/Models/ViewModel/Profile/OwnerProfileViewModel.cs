using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models.ViewModel.Profile
{
    public class OwnerProfileViewModel
    {
        public string UserId { get; set; } = string.Empty;

        [Required(ErrorMessage = "First name is required.")]
        [StringLength(50)]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Last name is required.")]
        [StringLength(50)]
        public string LastName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public int WorkspaceId { get; set; }

        [Required(ErrorMessage = "Workspace name is required.")]
        [StringLength(150)]
        [Display(Name = "Workspace Name")]
        public string WorkspaceName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Address is required.")]
        public string Address { get; set; } = string.Empty;

        [Required(ErrorMessage = "Latitude is required.")]
        public double Latitude { get; set; }

        [Required(ErrorMessage = "Longitude is required.")]
        public double Longitude { get; set; }

        public string? Description { get; set; }

        public bool HasWiFi { get; set; }
        public bool HasAirConditioning { get; set; }
        public bool HasDrinksAndCafeteria { get; set; }

        [Range(0, 500)]
        public int TotalTables { get; set; }

        [Range(0, 100)]
        public int TotalRooms { get; set; }

        public List<ResourcePricingViewModel> Resources { get; set; } = new();
    }
}
