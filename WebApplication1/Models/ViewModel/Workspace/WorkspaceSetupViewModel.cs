using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models.ViewModels
{
    public class WorkspaceSetupViewModel
    {
        public string? PendingOwnerId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }

        [Required(ErrorMessage = "Workspace Name is required.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Address is required.")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Latitude is required.")]
        public double Latitude { get; set; }

        [Required(ErrorMessage = "Longitude is required.")]
        public double Longitude { get; set; }

        [Required(ErrorMessage = "Number of standard tables is required.")]
        [Range(0, 500, ErrorMessage = "Please enter a valid number.")]
        public int TotalTables { get; set; }

        [Required(ErrorMessage = "Number of meeting rooms is required.")]
        [Range(0, 100, ErrorMessage = "Please enter a valid number.")]
        public int TotalRooms { get; set; }

        [Required(ErrorMessage = "TablePricePerHour of tables rooms is required.")]
        public decimal TablePricePerHour { get; set; }
        [Required(ErrorMessage = "TablePricePerHour of meeting rooms is required.")]
        public decimal RoomPricePerHour { get; set; }
        public bool HasAirConditioning { get; set; }
        public bool HasWiFi { get; set; }
        public bool HasDrinksAndCafeteria { get; set; }

        public string? Description { get; set; }
    }
}