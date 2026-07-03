using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;

namespace WebApplication1.Repositories.Invoices
{
    public class InvoicesRepository : IInvoicesRepo
    {
        private readonly ApplicationDbContext _db;
        public InvoicesRepository(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<Invoice> CreateInvoiceAsync(int bookingId, string paymentMethod)
        {
            Booking booking = await _db.Bookings.Include(b => b.Orders).ThenInclude(o=>o.OrderDetails)
                .Include(b => b.Resourse)
                .FirstOrDefaultAsync(b => b.Id == bookingId);
            if (booking == null)
            {
                throw new Exception("Booking not found");
            }
            decimal totalHoursCost = 0;
            if (booking.EndTime.HasValue)
            {
                var duration = booking.EndTime.Value - booking.StartTime;
                double totalHours = Math.Ceiling(duration.TotalHours);
                totalHoursCost = (decimal)totalHours * 50;
            }
            decimal totalOrdersCost = booking.Orders?.SelectMany(o=>o.OrderDetails).Sum(od=> od.PriceAtSale * od.Quantity) ?? 0;
            decimal grandTotal = totalHoursCost + totalOrdersCost;
            Invoice invoice = new Invoice
            {
                BookingId = bookingId,
                IssueDate = DateTime.Now,
                TotalHoursCost = totalHoursCost,
                TotalOrdersCost = totalOrdersCost,
                GrandTotal = grandTotal,
                PaymentMethod = paymentMethod
            };
            await _db.Invoices.AddAsync(invoice);
            await _db.SaveChangesAsync();

            return invoice;

        }


        public async Task<IEnumerable<Invoice>> GetAllInvoicesAsync(int workspaceId)
        {
            return await _db.Invoices
                .Include(i => i.Booking)
                    .ThenInclude(b => b.User)
                .Include(i => i.Booking)
                    .ThenInclude(b => b.Resourse)
                .Where(i => i.WorkSpaceId == workspaceId) 
                .OrderByDescending(i => i.IssueDate)
                .ToListAsync();
        }

        public async Task<Invoice?> GetInvoiceByIdAsync(int id)
        {
            return await _db.Invoices.Include(i => i.Booking)
                    .ThenInclude(b => b.User)
                .Include(i => i.Booking)
                    .ThenInclude(b => b.Resourse)
                .Include(i => i.Booking)
                    .ThenInclude(b => b.Orders).ThenInclude(o => o.OrderDetails)
                    .ThenInclude(d => d.MenuItem).FirstOrDefaultAsync(i => i.Id == id);

        }

        public async Task<IEnumerable<Invoice>> GetInvoicesByUserIdAsync(string userId)
        {
            return await _db.Invoices
             .Include(i => i.Booking)
             .Where(i => i.Booking.UserId == userId)
             .ToListAsync();
        }

        public async Task<decimal> GetTotalRevenueAsync()
        {
            return await _db.Invoices.SumAsync(i => i.GrandTotal);
        }

        public async Task<bool> UpdatePaymentMethodAsync(int invoiceId, string newPaymentMethod)
        {
            var invoice = await _db.Invoices.FindAsync(invoiceId);
            if (invoice == null) return false;

            invoice.PaymentMethod = newPaymentMethod;
            await _db.SaveChangesAsync();
            return true;
        }
    }
}
