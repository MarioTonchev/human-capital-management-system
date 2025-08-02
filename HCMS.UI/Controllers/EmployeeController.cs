using HCMS.Core.DTOs.Employee;
using HCMS.Core.Contracts;
using HCMS.UI.ViewModels.Employee;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HCMS.UI.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IEmployeeUIService employeeUIService;
        private readonly IDepartmentUIService departmentUIService;

        public EmployeeController(IEmployeeUIService employeeUIService, IDepartmentUIService departmentUIService)
        {
            this.employeeUIService = employeeUIService;
            this.departmentUIService = departmentUIService;
        }

        public async Task<IActionResult> MyProfile()
        {
            var employee = await employeeUIService.GetMyProfileAsync();

            if (employee == null)
            {
                return RedirectToAction("Login", "Account");
            }

            return View("Details", employee); 
        }

        public async Task<IActionResult> Index()
        {
            var employees = await employeeUIService.GetAllEmployeesAsync();
            return View(employees);
        }

        public async Task<IActionResult> Details(int id)
        {
            var employee = await employeeUIService.GetEmployeeByIdAsync(id);

            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var employee = await employeeUIService.GetEmployeeByIdAsync(id);

            if (employee == null)
            {
                return NotFound();
            }

            var editViewModel = new EmployeeEditViewModel
            {
                Id = employee.Id,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                Email = employee.Email,
                JobTitle = employee.JobTitle,
                Salary = employee.Salary,
                DepartmentId = employee.DepartmentId
            };

            var departments = await departmentUIService.GetAllDepartmentsAsync();
            ViewBag.Departments = new SelectList(departments, "Id", "Name", editViewModel.DepartmentId);

            return View(editViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EmployeeEditViewModel edited)
        {
            if (!ModelState.IsValid)
            {
                var departments = await departmentUIService.GetAllDepartmentsAsync();
                ViewBag.Departments = new SelectList(departments, "Id", "Name", edited.DepartmentId);

                return View(edited);
            }

            var dtoEdited = new UpdateEmployeeDto
            {
                FirstName = edited.FirstName,
                LastName = edited.LastName,
                Email = edited.Email,
                JobTitle = edited.JobTitle,
                Salary = edited.Salary,
                DepartmentId = edited.DepartmentId
            };

            bool updated = await employeeUIService.UpdateEmployeeAsync(id, dtoEdited);

            if (!updated)
            {
                var departments = await departmentUIService.GetAllDepartmentsAsync();
                ViewBag.Departments = new SelectList(departments, "Id", "Name", edited.DepartmentId);

                ModelState.AddModelError("", "Failed to update employee.");
                return View(edited);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
