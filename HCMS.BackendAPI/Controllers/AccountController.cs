using HCMS.Core.Contracts;
using HCMS.Core.DTOs.Account;
using HCMS.Core.DTOs.Employee;
using HCMS.Infrastructure.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HCMS.BackendAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IEmployeeService employeeService;
        private readonly IJWTService jwtService;
        private readonly UserManager<ApplicationUser> userManager;

        public AccountController(
            IEmployeeService employeeService,
            IJWTService jwtService,
            UserManager<ApplicationUser> userManager)
        {
            this.employeeService = employeeService;
            this.jwtService = jwtService;
            this.userManager = userManager;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await userManager.FindByNameAsync(dto.Username);

            if (user == null || !await userManager.CheckPasswordAsync(user, dto.Password))
            {
                return Unauthorized("Invalid username or password");
            }

            var roles = await userManager.GetRolesAsync(user);
            var token = jwtService.GenerateToken(user, roles);

            return Ok(token);
        }

        [Authorize(Roles = "HRAdmin")]
        [HttpPost("register-user")]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterUserDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createEmployeeDto = new CreateEmployeeDto
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                JobTitle = dto.JobTitle,
                Salary = dto.Salary,
                DepartmentId = dto.DepartmentId
            };

            var createdEmployee = await employeeService.CreateAsync(createEmployeeDto);

            var newApplicationUser = new ApplicationUser
            {
                UserName = dto.Username,
                Email = dto.Email,
                EmployeeId = createdEmployee.Id
            };

            var result = await userManager.CreateAsync(newApplicationUser, dto.Password);

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            await userManager.AddToRoleAsync(newApplicationUser, dto.Role);
            return Ok("User created");
        }

        [Authorize(Roles = "HRAdmin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var appUser = await userManager.Users.FirstOrDefaultAsync(u => u.EmployeeId == id);

            if (appUser == null)
            {
                return NotFound();
            }

            var isApplicationUserDeleted = await userManager.DeleteAsync(appUser);

            if (!isApplicationUserDeleted.Succeeded)
            {
                return BadRequest(isApplicationUserDeleted.Errors);
            }

            var isEmployeeDeleted = await employeeService.DeleteAsync(id);

            if (!isEmployeeDeleted)
            {
                return NotFound();
            }

            return NoContent();
        }

        [Authorize]
        [HttpPost("logout")]
        public IActionResult Logout()
        {
            return Ok("Logged out");
        }
    }
}