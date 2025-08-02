using HCMS.Core.DTOs.Department;

namespace HCMS.Core.Contracts
{
    public interface IDepartmentUIService
    {
        Task<List<DepartmentDto>> GetAllDepartmentsAsync();
        Task<DepartmentDto> GetDepartmentByIdAsync(int id);
        Task<bool> CreateDepartmentAsync(CreateDepartmentDto dto);
        Task<bool> UpdateDepartmentAsync(int id, UpdateDepartmentDto dto);
        Task<bool> DeleteDepartmentAsync(int id);
    }
}
