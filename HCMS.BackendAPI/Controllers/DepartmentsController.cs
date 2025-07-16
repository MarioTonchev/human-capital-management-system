using HCMS.Core.Contracts;
using HCMS.Core.DTOs.Department;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HCMS.BackendAPI.Controllers
{
    [Authorize(Roles = "HRAdmin,Manager")]
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentsController : ControllerBase
    {
        private readonly IDepartmentService departmentService;
        private readonly ICurrentUserService currentUserService;

        public DepartmentsController(IDepartmentService departmentService, ICurrentUserService currentUserService)
        {
            this.departmentService = departmentService;
            this.currentUserService = currentUserService;
        }

        [Authorize(Roles = "HRAdmin")]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var departments = await departmentService.GetAllAsync();

            return Ok(departments);
        }

        [Authorize(Roles = "HRAdmin,Manager")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var currUser = await currentUserService.GetCurrentUserAsync();
            var dept = await departmentService.GetByIdAsync(id);

            if (dept == null)
            {
                return NotFound();
            }

            if (User.IsInRole("HRAdmin"))
            {
                return Ok(dept);
            }

            if (User.IsInRole("Manager"))
            {
                if (dept.Id == currUser.Employee.DepartmentId)
                {
                    return Ok(dept);
                }
            }

            return Forbid();
        }

        [Authorize(Roles = "HRAdmin")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateDepartmentDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var created = await departmentService.CreateAsync(dto);

            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [Authorize(Roles = "HRAdmin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] CreateDepartmentDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var updated = await departmentService.UpdateAsync(id, dto);

            if (!updated)
            {
                return NotFound();
            }

            return NoContent();
        }

        [Authorize(Roles = "HRAdmin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await departmentService.DeleteAsync(id);

            if (!deleted)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
