using HCMS.Core.Contracts;
using HCMS.Core.DTOs.Department;
using HCMS.Core.DTOs.Employee;
using HCMS.Infrastructure.Entities;
using HCMS.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace HCMS.Core.Services
{
    public class DepartmentService : IDepartmentService
    {
        private readonly IRepository repository;

        public DepartmentService(IRepository repository)
        {
            this.repository = repository;
        }

        public async Task<IEnumerable<DepartmentDto>> GetAllAsync()
        {
            var departments = await repository.All<Department>().Include(d => d.Employees).ToListAsync();

            return departments.Select(MapToDto);
        }

        public async Task<DepartmentDto?> GetByIdAsync(int id)
        {
            var dept = await repository.All<Department>().Include(d => d.Employees)
                .FirstOrDefaultAsync(d => d.Id == id);

            if (dept == null)
            {
                return null;
            }

            return MapToDto(dept);
        }

        public async Task<DepartmentDto> CreateAsync(CreateDepartmentDto dto)
        {
            var dept = new Department
            {
                Name = dto.Name
            };

            await repository.AddAsync(dept);

            await repository.SaveChangesAsync();

            var result = new DepartmentDto
            {
                Id = dept.Id,
                Name = dto.Name
            };

            return result;
        }

        public async Task<bool> UpdateAsync(int id, UpdateDepartmentDto dto)
        {
            var dept = await repository.GetByIdAsync<Department>(id);

            if (dept == null)
            {
                return false;
            }

            dept.Name = dto.Name;

            await repository.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var allDepartments = await repository.All<Department>().Include(d => d.Employees).ToListAsync();
            var dept = allDepartments.FirstOrDefault(d => d.Id == id);

            if (dept == null)
            {
                return false;
            }

            if (dept.Employees.Any())
            {
                return false;
            }

            repository.Delete(dept);

            await repository.SaveChangesAsync();

            return true;
        }

        private DepartmentDto MapToDto(Department department)
        {
            var result = new DepartmentDto
            {
                Id = department.Id,
                Name = department.Name,
                Employees = department.Employees?.Select(e => new EmployeeDto
                {
                    Id = e.Id,
                    FirstName = e.FirstName,
                    LastName = e.LastName,
                    Email = e.Email,
                    JobTitle = e.JobTitle,
                    Salary = e.Salary
                }).ToList() ?? new List<EmployeeDto>()
            };

            return result;
        }
    }
}
