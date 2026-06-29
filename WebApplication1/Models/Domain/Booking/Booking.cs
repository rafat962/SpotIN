using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebApplication1.Models.Domain.User;

public class Booking
{
    public int Id { get; set; }

    public string? UserId { get; set; }
    [ForeignKey("UserId")]
    public virtual ApplicationUser? User { get; set; }

    public int ResourceId { get; set; }
    [ForeignKey("ResourceId")]
    public virtual Resourse Resourse { get; set; }

    [Required]
    public DateTime StartTime { get; set; } = DateTime.Now;

    public DateTime? EndTime { get; set; }

    [Required]
    public string Status { get; set; } = "Active"; // Active, Completed, Cancelled

    // Navigation Properties
    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
    public virtual ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();
}