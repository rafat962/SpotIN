using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using preSpotIn.Enums;
using WebApplication1.Models.Domain.User;

namespace preSpotIn.Models
{
    public class WalletTransaction
    {
        public int Id { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

        [Required]
        public TransactionType Type { get; set; } // Enum

        [Required]
        public DateTime TransactionDate { get; set; } = DateTime.Now;

        [Required]
        public string UserId { get; set; } // مضبوطة string وجاهزة

        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; }

        public int? BookingId { get; set; }

        [ForeignKey("BookingId")]
        public virtual Booking Booking { get; set; }
    }
}