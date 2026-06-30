using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models.Domain.User;
using preSpotIn.Models; 

namespace WebApplication1.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<WorkSpace> WorkSpaces { get; set; }
        public DbSet<Resourse> Resourses { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<MenuItem> MenuItems { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetails> OrderDetails { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<WalletTransaction> WalletTransactions { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // 1. Seed Roles
            string ownerRoleId = "1a2b3c4d-5e6f-7a8b-9c0d-1e2f3a4b5c6d";
            string clientRoleId = "2b3c4d5e-6f7a-8b9c-0d1e-2f3a4b5c6d7e";

            builder.Entity<IdentityRole>().HasData(
                new IdentityRole { Id = ownerRoleId, Name = "Owner", NormalizedName = "OWNER" },
                new IdentityRole { Id = clientRoleId, Name = "Client", NormalizedName = "CLIENT" }
            );

            // 2. Seed Users
            string ownerUserId = "3c4d5e6f-7a8b-9c0d-1e2f-3a4b5c6d7e8f";
            string clientUserId = "4d5e6f7a-8b9c-0d1e-2f3a4b5c6d7e8f9a";

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
                WalletBalance = 500.00m,
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
                WalletBalance = 150.00m,
                SecurityStamp = Guid.NewGuid().ToString()
            };
            clientUser.PasswordHash = hasher.HashPassword(clientUser, "Client@123");

            builder.Entity<ApplicationUser>().HasData(ownerUser, clientUser);

            // 3. Seed UserRoles
            builder.Entity<IdentityUserRole<string>>().HasData(
                new IdentityUserRole<string> { RoleId = ownerRoleId, UserId = ownerUserId },
                new IdentityUserRole<string> { RoleId = clientRoleId, UserId = clientUserId }
            );
            builder.Entity<OrderDetails>()
                .HasOne(od => od.Order)
                .WithMany(o => o.OrderDetails)
                .HasForeignKey(od => od.OrderId)
                .OnDelete(DeleteBehavior.Restrict);

            // 4. SEED DATA
            DataSeeder.Seed(builder, ownerUserId, clientUserId);
        }
    }
}