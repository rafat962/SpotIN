using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers.Owner
{
    [Authorize(Roles = "Owner")]

    public class MenuController : Controller
    {
        public IActionResult MenuList()
        {
            var workspaceId = User.FindFirst("WorkspaceId")?.Value;

            return View("MenuList");
        }
        public IActionResult CreateMenu()
        {
            return View("CreateMenu");
        }
    }
}
