using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis;

namespace WebApplication1.Models.Domain.User
{
    public class ApplicationUser : IdentityUser
    {
        [Required(ErrorMessage = "First Name is required.")]
        [StringLength(50)]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Last Name is required.")]
        [StringLength(50)]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Wallet Balance is required.")]
        [Range(0, 999999, ErrorMessage = "Wallet Balance must be a positive value.")]
        [Column(TypeName = "decimal(18,2)")] // تم التعديل هنا لضمان دقة الحسابات
        public decimal WalletBalance { get; set; }

        // Navigation Properties - تم تعديل الـ S لتكون كابيتال لتطابق الكلاس
        public virtual ICollection<WorkSpace> OwnedWorkspaces { get; set; } = new List<WorkSpace>();
        public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    }
}
