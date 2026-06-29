using Microsoft.EntityFrameworkCore;
using preSpotIn.Models;

namespace WebApplication1.Data
{
    public static class DataSeeder
    {
        public static void Seed(ModelBuilder builder, string ownerId, string clientId)
        {
            // 1. Seed WorkSpace
            int workspaceId = 1;
            builder.Entity<WorkSpace>().HasData(new WorkSpace
            {
                Id = workspaceId,
                Name = "SpotIn Dokki Branch",
                Address = "12 Tahrir Street, Dokki, Giza",
                Latitude = 30.0384,
                Longitude = 31.2122,
                Description = "Cozy workspace with premium high-speed internet and silent rooms.",
                HasWiFi = true,
                HasAirConditioning = true,
                OwnerId = ownerId
            });

            // 2. Seed Resourses 
            int tableResourceId = 1;
            int roomResourceId = 2;
            builder.Entity<Resourse>().HasData(
                new Resourse
                {
                    Id = tableResourceId,
                    Name = "Red Table - Open Space",
                    Type = "Table",
                    HourlyRate = 20.00m,
                    IsAvailable = false, 
                    WorkspaceId = workspaceId
                },
                new Resourse
                {
                    Id = roomResourceId,
                    Name = "Meeting Room A",
                    Type = "MeetingRoom",
                    HourlyRate = 100.00m,
                    IsAvailable = true,
                    WorkspaceId = workspaceId
                }
            );

            // 3. Seed MenuItem 
            int coffeeItemId = 1;
            int waterItemId = 2;
            builder.Entity<MenuItem>().HasData(
                new MenuItem { Id = coffeeItemId, Name = "Turkish Coffee", Price = 25.00m, IsAvailable = true, WorkSpaceID = workspaceId },
                new MenuItem { Id = waterItemId, Name = "Mineral Water", Price = 10.00m, IsAvailable = true, WorkSpaceID = workspaceId }
            );

            // 4. Seed Booking 
            int bookingId = 1;
            builder.Entity<Booking>().HasData(new Booking
            {
                Id = bookingId,
                UserId = clientId,
                ResourceId = tableResourceId,
                StartTime = DateTime.Parse("2026-06-29T18:00:00"),
                EndTime = null, 
                Status = "Active"
            });

            // 5. Seed Order 
            int orderId = 1;
            builder.Entity<Order>().HasData(new Order
            {
                Id = orderId,
                BookingId = bookingId,
                OrderDate = DateTime.Parse("2026-06-29T18:30:00"),
                OrderStatus = "Delivered"
            });

            // 6. Seed OrderDetails 
            builder.Entity<OrderDetails>().HasData(
                new OrderDetails { Id = 1, OrderId = orderId, MenuItemId = coffeeItemId, Quantity = 2, PriceAtSale = 25.00m },
                new OrderDetails { Id = 2, OrderId = orderId, MenuItemId = waterItemId, Quantity = 1, PriceAtSale = 10.00m }
            );

            // 7. Seed Invoice 
            builder.Entity<Invoice>().HasData(new Invoice
            {
                Id = 1,
                BookingId = bookingId,
                IssueDate = DateTime.Parse("2026-06-29T21:00:00"),
                TotalHoursCost = 60.00m, 
                TotalOrdersCost = 60.00m,
                GrandTotal = 120.00m,
                PaymentMethod = "Cash"
            });
        }
    }
}