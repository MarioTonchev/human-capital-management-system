using HCMS.Core.DTOs.Account;
using HCMS.UI.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace HCMS.UI.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountUIService accountService;

        public AccountController(IAccountUIService accountService)
        {
            this.accountService = accountService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return View(dto);
            }

            bool success = await accountService.LoginAsync(dto);
            if (!success)
            {
                ModelState.AddModelError("", "Invalid login.");
                return View(dto);
            }

            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Logout()
        {
            await accountService.LogoutAsync();
            return RedirectToAction("Index", "Home"); 
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterUserDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return View(dto);
            }

            bool success = await accountService.RegisterAsync(dto);

            if (!success)
            {
                ModelState.AddModelError("", "Registration failed.");
                return View(dto);
            }

            return RedirectToAction("Login");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            bool success = await accountService.DeleteUserAsync(id);

            if (!success)
            {
                return NotFound("User not found or couldn't be deleted.");
            }

            return RedirectToAction("Index", "Home");
        }
    }
}
