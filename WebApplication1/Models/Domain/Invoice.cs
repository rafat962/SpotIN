using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Invoice
{
    public int Id { get; set; }

    public int BookingId { get; set; }
    [ForeignKey("BookingId")]
    public virtual Booking? Booking { get; set; }

    public DateTime IssueDate { get; set; } = DateTime.Now;

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal TotalHoursCost { get; set; }

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal TotalOrdersCost { get; set; }

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal GrandTotal { get; set; }

    [Required(ErrorMessage = "Payment method is required")]
    public string? PaymentMethod { get; set; }
}