using HCMS.Core.DTOs.Account;
using HCMS.Core.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HCMS.UI.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountUIService accountUIService;
        private readonly IDepartmentUIService departmentUIService;

        public AccountController(IAccountUIService accountService, IDepartmentUIService departmentUIService)
        {
            this.accountUIService = accountService;
            this.departmentUIService = departmentUIService;
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

            bool success = await accountUIService.LoginAsync(dto);
            if (!success)
            {
                ModelState.AddModelError("", "Invalid login.");
                return View(dto);
            }

            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Logout()
        {
            await accountUIService.LogoutAsync();
            return RedirectToAction("Index", "Home"); 
        }

        [HttpGet]
        public async Task<IActionResult> RegisterUser()
        {
            var departments = await departmentUIService.GetAllDepartmentsAsync();

            ViewBag.Departments = new SelectList(departments, "Id", "Name");
            ViewBag.Roles = new SelectList(new[] { "Employee", "Manager", "HRAdmin" });

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegisterUser(RegisterUserDTO dto)
        {
            if (!ModelState.IsValid)
            {
                var departments = await departmentUIService.GetAllDepartmentsAsync();
                ViewBag.Departments = new SelectList(departments, "Id", "Name");
                ViewBag.Roles = new SelectList(new[] { "Employee", "Manager", "HRAdmin" });

                return View(dto);
            }

            bool success = await accountUIService.RegisterAsync(dto);

            if (!success)
            {
                var departments = await departmentUIService.GetAllDepartmentsAsync();
                ViewBag.Departments = new SelectList(departments, "Id", "Name");
                ViewBag.Roles = new SelectList(new[] { "Employee", "Manager", "HRAdmin" });

                ModelState.AddModelError("", "Registration failed.");
                return View(dto);
            }

            return RedirectToAction("Index", "Employee");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await accountUIService.DeleteUserAsync(id);

            return RedirectToAction("Index", "Employee");
        }
    }
}
