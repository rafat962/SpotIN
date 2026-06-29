using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Order
{
    public int Id { get; set; }

    public int BookingId { get; set; }
    [ForeignKey("BookingId")]
    public virtual Booking Booking { get; set; }

    public DateTime OrderDate { get; set; } = DateTime.Now;

    [Required]
    public string OrderStatus { get; set; } = "Pending"; // Pending, Delivered, Paid

    // Navigation Properties
    public virtual ICollection<OrderDetails> OrderDetails { get; set; } = new List<OrderDetails>();
}

