using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers.Owner
{
    public class WorkSpaceController : Controller
    {
        public IActionResult MangeWorkSpace()
        {
            return View("MangeWorkSpace");
        }
    }
}
