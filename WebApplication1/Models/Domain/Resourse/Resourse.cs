using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Resourse
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Resource name is required")]
    [StringLength(100, ErrorMessage = "Resource name cannot exceed 100 characters")]
    public string Name { get; set; }

    [Required(ErrorMessage = "Resource type is required")]
    public string Type { get; set; } // Table, MeetingRoom, PrivateOffice

    [Required(ErrorMessage = "Hourly rate is required")]
    [Column(TypeName = "decimal(18,2)")]
    public decimal HourlyRate { get; set; }

    public bool IsAvailable { get; set; } = true;

    // Foreign Key to Workspace
    public int WorkspaceId { get; set; }
    [ForeignKey("WorkspaceId")]
    public virtual WorkSpace WorkSpace { get; set; }

    // Navigation Properties
    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();
}