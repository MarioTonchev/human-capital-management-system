using HCMS.Core.Contracts;
using HCMS.Core.DTOs.Employee;
using HCMS.Infrastructure.Entities;
using HCMS.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace HCMS.Core.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IRepository repository;

        public EmployeeService(IRepository repository)
        {
            this.repository = repository;
        }

        public async Task<IEnumerable<EmployeeDto>> GetAllAsync()
        {
            var employees = await repository.All<Employee>().ToListAsync();

            return employees.Select(MapToDto);
        }

        public async Task<EmployeeDto?> GetByIdAsync(int id)
        {
            var employee = await repository.GetByIdAsync<Employee>(id);

            if (employee == null)
            {
                return null;
            }

            return MapToDto(employee);
        }

        public async Task<EmployeeDto> CreateAsync(CreateEmployeeDto dto)
        {
            var employee = new Employee
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                DepartmentId = dto.DepartmentId,
                Salary = dto.Salary,
                JobTitle = dto.JobTitle
            };

            await repository.AddAsync(employee);
            await repository.SaveChangesAsync();

            var result = new EmployeeDto
            {
                Id = employee.Id,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                Email = employee.Email,
                JobTitle = employee.JobTitle,
                Salary = employee.Salary,
                DepartmentId = employee.DepartmentId
            };

            return result;
        }

        public async Task<bool> UpdateAsync(int id, UpdateEmployeeDto dto)
        {
            var employee = await repository.GetByIdAsync<Employee>(id);

            if (employee == null)
            {
                return false;
            }

            employee.FirstName = dto.FirstName;
            employee.LastName = dto.LastName;
            employee.Email = dto.Email;
            employee.Salary = dto.Salary;
            employee.JobTitle = dto.JobTitle;
            employee.DepartmentId = dto.DepartmentId;

            await repository.SaveChangesAsync();
            
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var employee = await repository.GetByIdAsync<Employee>(id);

            if (employee == null)
            {
                return false;
            }

            repository.Delete(employee);

            await repository.SaveChangesAsync();

            return true;
        }

        public async Task<IEnumerable<EmployeeDto>> GetByDepartmentIdAsync(int departmentId)
        {
            var employees = await repository.All<Employee>()
                .Where(e => e.DepartmentId == departmentId)
                .ToListAsync();

            return employees.Select(MapToDto);
        }

        private EmployeeDto MapToDto(Employee employee) => new EmployeeDto
        {
            Id = employee.Id,
            FirstName = employee.FirstName,
            LastName = employee.LastName,
            Email = employee.Email,
            DepartmentId = employee.DepartmentId,
            Salary = employee.Salary,
            JobTitle = employee.JobTitle
        };
    }
}
