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

        public async Task CreateAsync(CreateEmployeeDto dto)
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
        }

        public async Task UpdateAsync(int id, CreateEmployeeDto dto)
        {
            var employee = await repository.GetByIdAsync<Employee>(id);

            if (employee == null)
            {
                throw new KeyNotFoundException("Employee not found.");
            }

            employee.FirstName = dto.FirstName;
            employee.LastName = dto.LastName;
            employee.Email = dto.Email;
            employee.DepartmentId = dto.DepartmentId;
            employee.Salary = dto.Salary;
            employee.JobTitle = dto.JobTitle;

            await repository.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var employee = await repository.GetByIdAsync<Employee>(id);

            if (employee == null)
            {
                throw new KeyNotFoundException("Employee not found.");
            }

            repository.Delete(employee);

            await repository.SaveChangesAsync();
        }

        public async Task<IEnumerable<EmployeeDto>> GetByDepartmentIdAsync(int departmentId)
        {
            var employees = await repository.All<Employee>()
                .Where(e => e.DepartmentId == departmentId)
                .ToListAsync();

            return employees.Select(MapToDto);
        }

        public async Task<IEnumerable<EmployeeDto>> GetByDepartmentNameAsync(string departmentName)
        {
            var employees = await repository.All<Employee>()
                .Where(e => e.Department.Name == departmentName)
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
