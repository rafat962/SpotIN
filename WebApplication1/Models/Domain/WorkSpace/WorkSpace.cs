using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebApplication1.Models.Domain.User;

public class WorkSpace
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Workspace name is required.")]
    [StringLength(150, ErrorMessage = "Workspace name cannot exceed 150 characters.")]
    public string Name { get; set; }

    [Required(ErrorMessage = "Address is required.")]
    public string Address { get; set; }

    public double Latitude { get; set; }
    public double Longitude { get; set; }

    public string? Description { get; set; }

    // Amenities
    public bool HasWiFi { get; set; }
    public bool HasAirConditioning { get; set; }
    public bool HasDrinksAndCafeteria { get; set; } 

    // Capacity 
    public int TotalTables { get; set; } 
    public int TotalRooms { get; set; }  

    // Foreign Key to Identity User
    public string OwnerId { get; set; }
    [ForeignKey("OwnerId")]
    public virtual ApplicationUser Owner { get; set; }

    public virtual ICollection<Resourse> Resourses { get; set; } = new List<Resourse>();
}