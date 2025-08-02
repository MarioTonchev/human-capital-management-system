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

        /// <summary>
        /// Login endpoint for users. Validates the username and password, and returns a JWT token if successful.
        /// </summary>
        /// <param name="dto">Form data mapped to login dto.</param>
        /// <returns>JWT if successful.</returns>
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

        /// <summary>
        /// Register a new user in the system. This endpoint is only accessible by HRAdmins.
        /// </summary>
        /// <param name="dto">Form data mapped to register user dto.</param>
        /// <returns>Status code depending on whether the registration was successful or a failure.</returns>
        [Authorize(Roles = "HRAdmin")]
        [HttpPost("register-user")]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterUserDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (await userManager.FindByNameAsync(dto.Username) != null)
            {
                return BadRequest("Username already exists.");
            }

            if (await userManager.FindByEmailAsync(dto.Email) != null)
            {
                return BadRequest("Email already in use.");
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

        /// <summary>
        /// Delete a user from the system. This endpoint is only accessible by HRAdmins.
        /// </summary>
        /// <param name="id">The id of the user.</param>
        /// <returns>Status code depending on whether the deletion was successful or a failure.</returns>
        [Authorize(Roles = "HRAdmin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var appUser = await userManager.Users.FirstOrDefaultAsync(u => u.EmployeeId == id);

            if (appUser == null)
            {
                return NotFound();
            }

            var currentUserId = userManager.GetUserId(User);

            if (appUser.Id == currentUserId)
            {
                return BadRequest("You cannot delete your own account.");
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
    }
}