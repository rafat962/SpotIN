using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models.ViewModel.Account
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "First name is required")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Last name is required")]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email address is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; } = string.Empty;

        public string? Organization { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [MinLength(4, ErrorMessage = "Password must be at least 4 characters")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Account type selection is required")]
        public string Role { get; set; } = "Client";
    }
}
