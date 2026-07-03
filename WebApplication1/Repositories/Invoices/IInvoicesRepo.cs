namespace WebApplication1.Repositories.Invoices
{
    public interface IInvoicesRepo
    {
        // Define methods for invoice-related operations
        //Get all invoices 
        Task<IEnumerable<Invoice>> GetAllInvoicesAsync(int workspaceId);
        //Get invoice by ID
        Task<Invoice?> GetInvoiceByIdAsync(int id);
        //Get invoices by user ID
        Task<IEnumerable<Invoice>> GetInvoicesByUserIdAsync(string userId);
        //create invoice for a booking
        Task<Invoice> CreateInvoiceAsync(int bookingId, string paymentMethod);
        //Update payment method for an invoice
        Task<bool> UpdatePaymentMethodAsync(int invoiceId, string newPaymentMethod);
        //Total revenue for all invoices
        Task<decimal> GetTotalRevenueAsync();
    }
}
