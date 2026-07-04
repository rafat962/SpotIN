using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models.ViewModel.Profile
{
    public class ResourcePricingViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Resource name is required.")]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        public string Type { get; set; } = string.Empty;

        [Required(ErrorMessage = "Hourly rate is required.")]
        [Range(0.01, 100000, ErrorMessage = "Hourly rate must be greater than zero.")]
        public decimal HourlyRate { get; set; }

        public bool IsAvailable { get; set; }
    }
}
