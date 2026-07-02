using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Data;
using WebApplication1.Repositories.Invoices;

namespace WebApplication1.Controllers.Owner
{
    [Authorize(Roles = "Owner")]
    public class InvoicesController : Controller
    {
        private readonly IInvoicesRepo _invoiceRepo;
        public InvoicesController(IInvoicesRepo invoiceRepo)
        {
            _invoiceRepo = invoiceRepo;
        }
        public async Task<IActionResult> InvoicesList(string searchString, string paymentMethod)
        {
            var invoices = await _invoiceRepo.GetAllInvoicesAsync();
            if (!string.IsNullOrEmpty(searchString))
            {
                invoices = invoices.Where(i => i.Booking?.User?.UserName != null &&
                                               i.Booking.User.UserName.Contains(searchString, StringComparison.OrdinalIgnoreCase));
            }
            if (!string.IsNullOrEmpty(paymentMethod))
            {
                invoices = invoices.Where(i => i.PaymentMethod == paymentMethod);
            }
            ViewData["CurrentSearch"] = searchString;
            ViewData["CurrentPaymentMethod"] = paymentMethod;
            return View(invoices);
        }
        [HttpGet]
        public async Task<IActionResult> GetDetails(int id)
        {
            var invoice = await _invoiceRepo.GetInvoiceByIdAsync(id);

            if (invoice == null)
            {
                return NotFound();
            }
            return PartialView("_InvoiceDetailsPartial", invoice);
        }
        //public IActionResult InvoicesList()
        //{
        //    var workspaceId = User.FindFirst("WorkspaceId")?.Value;

        //    return View("InvoicesList");
        //}
    }
}
