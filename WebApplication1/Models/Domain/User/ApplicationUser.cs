using Microsoft.AspNetCore.Identity;

namespace WebApplication1.Models.Domain.User
{
    public class ApplicationUser:IdentityUser
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
    }
}
