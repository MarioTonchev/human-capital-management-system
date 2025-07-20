using HCMS.Core.Contracts;
using HCMS.Core.DTOs.Employee;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HCMS.BackendAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    [Tags("Employees")]
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeeService employeeService;
        private readonly ICurrentUserService currentUserService;

        public EmployeesController(IEmployeeService employeeService, ICurrentUserService currentUserService)
        {
            this.employeeService = employeeService;
            this.currentUserService = currentUserService;
        }

        /// <summary>
        /// Get the profile of the currently logged-in user.
        /// </summary>
        /// <returns>Information about the currently logged-in user as an employee dto.</returns>
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

        /// <summary>
        /// Get all employees in the system. HRAdmins can access all employees, while Managers can only access employees from their own department.
        /// </summary>
        /// <returns>Collection of all employees</returns>
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

            if (User.IsInRole("Manager"))
            {
                var employeesByDepartment = await employeeService.GetByDepartmentIdAsync((int)currUser.Employee.DepartmentId);
                return Ok(employeesByDepartment);
            }

            return Forbid();
        }

        /// <summary>
        /// Get a specific employee by their ID. HRAdmins can access any employee, while Managers can only access employees from their own department.
        /// </summary>
        /// <param name="id">The id of the employee.</param>
        /// <returns>The found employee as an employee dto.</returns>
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

        /// <summary>
        /// Update an employee's information. HRAdmins can update any employee, while Managers can only update employees from their own department.
        /// </summary>
        /// <param name="id">The id of the employee.</param>
        /// <param name="dto">Form data mapped to an update employee dto</param>
        /// <returns>Status code without any data depending on whether the update has succeeded or failed.</returns>
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
