using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers.Owner
{
    [Authorize(Roles = "Owner")]
    public class OwnerController : Controller
    {
        public IActionResult Index()
        {
            ViewData["PageTitle"] = "Owner Dashboard";
            ViewData["Breadcrumb"] = "SpotIN · Owner Portal";
            return View();
        }
    }
}
