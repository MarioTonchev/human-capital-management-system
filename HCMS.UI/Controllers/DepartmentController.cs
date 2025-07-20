using HCMS.Core.DTOs.Department;
using HCMS.UI.Contracts;
using HCMS.UI.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace HCMS.UI.Controllers
{
    public class DepartmentController : Controller
    {
        private readonly IDepartmentUIService departmentUIService;

        public DepartmentController(IDepartmentUIService departmentUIService)
        {
            this.departmentUIService = departmentUIService;
        }

        public async Task<IActionResult> Index()
        {
            var departments = await departmentUIService.GetAllDepartmentsAsync();
            return View(departments);
        }

        public async Task<IActionResult> GetMyDepartment()
        {
            var departmentIdClaim = User.Claims.FirstOrDefault(c => c.Type == "DepartmentId")?.Value;

            if (departmentIdClaim != null)
            {
                int depId = int.Parse(departmentIdClaim);
                var department = await departmentUIService.GetDepartmentByIdAsync(depId);

                return View(department);
            }

            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Details(int id)
        {
            var department = await departmentUIService.GetDepartmentByIdAsync(id);

            if (department == null)
            {
                return NotFound();
            }

            return View(department);
        }

        [HttpGet]
        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateDepartmentDto dto)
        {
            if (!ModelState.IsValid) return View(dto);

            var result = await departmentUIService.CreateDepartmentAsync(dto);

            if (!result)
            {
                ModelState.AddModelError("", "Failed to create department.");
                return View(dto);
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var department = await departmentUIService.GetDepartmentByIdAsync(id);

            if (department == null)
            {
                return NotFound();
            }

            var editViewModel = new DepartmentEditViewModel
            {
                Id = department.Id,
                Name = department.Name
            };

            return View(editViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, DepartmentEditViewModel edited)
        {
            if (!ModelState.IsValid)
            {
                return View(edited);
            }

            var dto = new UpdateDepartmentDto
            {
                Name = edited.Name
            };

            var result = await departmentUIService.UpdateDepartmentAsync(id, dto);

            if (!result)
            {
                ModelState.AddModelError("", "Failed to update department.");
                return View(edited);
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            await departmentUIService.DeleteDepartmentAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
