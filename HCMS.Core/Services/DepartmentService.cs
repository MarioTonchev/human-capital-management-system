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

        public async Task<bool> UpdateAsync(int id, CreateDepartmentDto dto)
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
            var dept = await repository.GetByIdAsync<Department>(id);

            if (dept == null)
            {
                return false;
            }

            repository.Delete(dept);

            await repository.SaveChangesAsync();

            return true;
        }

        private DepartmentDto MapToDto(Department department) => new DepartmentDto
        {
            Id = department.Id,
            Name = department.Name
        };
    }
}
