using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers.Owner
{
    [Authorize(Roles = "Owner")]
    public class InvoicesController : Controller
    {
        public IActionResult InvoicesList()
        {
            var workspaceId = User.FindFirst("WorkspaceId")?.Value;

            return View("InvoicesList");
        }
    }
}
