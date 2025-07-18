using HCMS.Core.Contracts;
using HCMS.Core.DTOs.Employee;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HCMS.BackendAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeeService employeeService;
        private readonly ICurrentUserService currentUserService;

        public EmployeesController(IEmployeeService employeeService, ICurrentUserService currentUserService)
        {
            this.employeeService = employeeService;
            this.currentUserService = currentUserService;
        }

        [HttpGet("me")]
        public async Task<IActionResult> GetMyProfile()
        {
            var user = await currentUserService.GetCurrentUserAsync();

            if (user == null)
            {
                return Unauthorized();
            }

            var employee = await employeeService.GetByIdAsync((int)user.EmployeeId);

            return employee == null ? NotFound() : Ok(employee);
        }

        [Authorize(Roles = "HRAdmin,Manager")]
        [HttpGet]
        public async Task<IActionResult> GetAllEmployees()
        {
            var currUser = await currentUserService.GetCurrentUserAsync();
            var employees = await employeeService.GetAllAsync();

            if (User.IsInRole("HRAdmin"))
            {
                return Ok(employees);
            }

            //Here if the current user is a manager he will get only the employees from his own department
            if (User.IsInRole("Manager"))
            {
                var employeesByDepartment = await employeeService.GetByDepartmentIdAsync((int)currUser.Employee.DepartmentId);
                return Ok(employeesByDepartment);
            }

            return Forbid();
        }

        [Authorize(Roles = "HRAdmin,Manager")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetEmployeeById(int id)
        {
            var currUser = await currentUserService.GetCurrentUserAsync();
            var employeeToFind = await employeeService.GetByIdAsync(id);

            if (employeeToFind == null)
            {
                return NotFound();
            }

            if (User.IsInRole("HRAdmin"))
            {
                return Ok(employeeToFind);
            }

            if (User.IsInRole("Manager"))
            {
                if (employeeToFind.DepartmentId == currUser.Employee.DepartmentId)
                {
                    return Ok(employeeToFind);
                }
            }

            return Forbid();
        }

        [Authorize(Roles = "HRAdmin,Manager")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEmployee(int id, [FromBody] UpdateEmployeeDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var currUser = await currentUserService.GetCurrentUserAsync();

            if (currUser == null)
            {
                return Unauthorized();
            }

            bool isUpdated = false;

            if (User.IsInRole("HRAdmin"))
            {
                isUpdated = await employeeService.UpdateAsync(id, dto);
            }

            if (User.IsInRole("Manager"))
            {
                var userToUpdate = await employeeService.GetByIdAsync(id);

                if (userToUpdate.DepartmentId == currUser.Employee.DepartmentId)
                {
                    isUpdated = await employeeService.UpdateAsync(id, dto);
                }
            }

            if (!isUpdated)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
