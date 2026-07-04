using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using WebApplication1.Models.Domain.User;
using WebApplication1.Repositories.Menu;

namespace WebApplication1.Controllers.Owner
{
    [Authorize(Roles = "Owner")]
    public class MenuController : Controller
    {
        private readonly IMenuItemRepository _menuRepository;
        private readonly UserManager<ApplicationUser> _userManager;

        public MenuController(IMenuItemRepository menuRepository, UserManager<ApplicationUser> userManager)
        {
            _menuRepository = menuRepository;
            _userManager = userManager;
        }

        private int GetCurrentWorkspaceId()
        {
            var workspaceIdClaim = User.FindFirst("WorkspaceId")?.Value;

            if (string.IsNullOrEmpty(workspaceIdClaim))
            {
                return 1;
            }

            return int.Parse(workspaceIdClaim);
        }

        [HttpGet]
        public async Task<IActionResult> MenuList()
        {
            int workspaceId = GetCurrentWorkspaceId();

            var menuItems = await _menuRepository.GetByWorkspaceIdAsync(workspaceId);

            return View("MenuList", menuItems);
        }

        [HttpGet]
        public async Task<IActionResult> CreateMenu()
        {
            int workspaceId = GetCurrentWorkspaceId();

            bool hasMenu = await _menuRepository.HasMenuAsync(workspaceId);

            ViewBag.HasMenu = hasMenu;
            var emptyList = new List<MenuItem>();
            return View("CreateMenu", emptyList);
        }

        [HttpPost]
        public async Task<IActionResult> CreateMenu(List<MenuItem> items)
        {
            if (items == null || items.Count == 0)
            {
                ModelState.AddModelError("", "Cannot save an empty menu.");
                return View("CreateMenu", items ?? new List<MenuItem>());
            }

            int workspaceId = GetCurrentWorkspaceId();

            foreach (var item in items)
            {
                item.WorkSpaceID = workspaceId;
                item.IsAvailable = true;
            }

            await _menuRepository.AddMenuItemsAsync(items);

            return RedirectToAction(nameof(MenuList));
        }

        [HttpGet]
        public async Task<IActionResult> EditMenuItem(int id)
        {
            var item = await _menuRepository.GetByIdAsync(id);

            if (item == null || item.WorkSpaceID != GetCurrentWorkspaceId())
            {
                return NotFound();
            }

            return View("EditMenuItem", item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditMenuItem(MenuItem model)
        {
            if (string.IsNullOrEmpty(model.Name) || model.Price < 0)
            {
                ModelState.AddModelError("", "Please enter a valid Name and Price.");
                return View("EditMenuItem", model);
            }

            try
            {
                var existingItem = await _menuRepository.GetByIdAsync(model.Id);

                if (existingItem == null || existingItem.WorkSpaceID != GetCurrentWorkspaceId())
                {
                    return NotFound();
                }

                existingItem.Name = model.Name;
                existingItem.Price = model.Price;

                await _menuRepository.UpdateAsync(existingItem);

                return RedirectToAction(nameof(MenuList));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error occurred while saving: " + ex.Message);
                return View("EditMenuItem", model);
            }
        }

        [HttpPost]
        public async Task<IActionResult> ToggleAvailability(int id)
        {
            var item = await _menuRepository.GetByIdAsync(id);
            if (item == null) return Json(new { success = false });

            item.IsAvailable = !item.IsAvailable;
            await _menuRepository.UpdateAsync(item);

            return Json(new { success = true });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteMenuItem(int id)
        {
            var item = await _menuRepository.GetByIdAsync(id);
            if (item == null) return Json(new { success = false });

            await _menuRepository.DeleteAsync(id);
            return Json(new { success = true });
        }
    }
}