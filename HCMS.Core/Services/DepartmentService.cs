using HCMS.Core.Contracts;
using HCMS.Core.DTOs.Department;
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
            var departments = await repository.All<Department>().ToListAsync();

            return departments.Select(MapToDto);
        }

        public async Task<DepartmentDto?> GetByIdAsync(int id)
        {
            var dept = await repository.GetByIdAsync<Department>(id);

            if (dept == null)
            {
                return null;
            }

            return MapToDto(dept);
        }

        public async Task CreateAsync(CreateDepartmentDto dto)
        {
            var dept = new Department 
            {
                Name = dto.Name 
            };

            await repository.AddAsync(dept);

            await repository.SaveChangesAsync();
        }

        public async Task UpdateAsync(int id, CreateDepartmentDto dto)
        {
            var dept = await repository.GetByIdAsync<Department>(id);

            if (dept == null)
            {
                throw new KeyNotFoundException("Department not found.");
            }

            dept.Name = dto.Name;

            await repository.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var dept = await repository.GetByIdAsync<Department>(id);

            if (dept == null)
            {
                throw new KeyNotFoundException("Department not found.");
            }

            repository.Delete(dept);

            await repository.SaveChangesAsync();
        }

        private DepartmentDto MapToDto(Department department) => new DepartmentDto
        {
            Id = department.Id,
            Name = department.Name
        };
    }
}
