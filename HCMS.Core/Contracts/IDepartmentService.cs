using HCMS.Core.DTOs.Department;

namespace HCMS.Core.Contracts
{
    public interface IDepartmentService
    {
        Task<IEnumerable<DepartmentDto>> GetAllAsync();
        Task<DepartmentDto?> GetByIdAsync(int id);
        Task CreateAsync(CreateDepartmentDto dto);
        Task UpdateAsync(int id, CreateDepartmentDto dto);
        Task DeleteAsync(int id);
    }
}
