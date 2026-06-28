using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models.Domain.User;

namespace WebApplication1.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            string ownerRoleId = "1a2b3c4d-5e6f-7a8b-9c0d-1e2f3a4b5c6d";
            string clientRoleId = "2b3c4d5e-6f7a-8b9c-0d1e-2f3a4b5c6d7e";

            builder.Entity<IdentityRole>().HasData(
                new IdentityRole
                {
                    Id = ownerRoleId,
                    Name = "Owner",
                    NormalizedName = "OWNER"
                },
                new IdentityRole
                {
                    Id = clientRoleId,
                    Name = "Client",
                    NormalizedName = "CLIENT"
                }
            );

            string ownerUserId = "3c4d5e6f-7a8b-9c0d-1e2f-3a4b5c6d7e8f";
            string clientUserId = "4d5e6f7a-8b9c-0d1e-2f3a-4b5c6d7e8f9a";

            var hasher = new PasswordHasher<ApplicationUser>();

            var ownerUser = new ApplicationUser
            {
                Id = ownerUserId,
                FirstName = "Workspace",
                LastName = "Owner",
                UserName = "owner@spotin.com",
                NormalizedUserName = "OWNER@SPOTIN.COM",
                Email = "owner@spotin.com",
                NormalizedEmail = "OWNER@SPOTIN.COM",
                EmailConfirmed = true,
                SecurityStamp = Guid.NewGuid().ToString()
            };
            ownerUser.PasswordHash = hasher.HashPassword(ownerUser, "Owner@123");

            var clientUser = new ApplicationUser
            {
                Id = clientUserId,
                FirstName = "Normal",
                LastName = "Client",
                UserName = "client@spotin.com",
                NormalizedUserName = "CLIENT@SPOTIN.COM",
                Email = "client@spotin.com",
                NormalizedEmail = "CLIENT@SPOTIN.COM",
                EmailConfirmed = true,
                SecurityStamp = Guid.NewGuid().ToString()
            };
            clientUser.PasswordHash = hasher.HashPassword(clientUser, "Client@123");

            builder.Entity<ApplicationUser>().HasData(ownerUser, clientUser);

            builder.Entity<IdentityUserRole<string>>().HasData(
                new IdentityUserRole<string>
                {
                    RoleId = ownerRoleId,
                    UserId = ownerUserId
                },
                new IdentityUserRole<string>
                {
                    RoleId = clientRoleId,
                    UserId = clientUserId
                }
            );
        }
    }
}