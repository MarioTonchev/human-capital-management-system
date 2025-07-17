using HCMS.Core.DTOs.Employee;

namespace HCMS.Core.Contracts
{
    public interface IEmployeeService
    {
        Task<IEnumerable<EmployeeDto>> GetAllAsync();
        Task<EmployeeDto?> GetByIdAsync(int id);
        Task<EmployeeDto> CreateAsync(CreateEmployeeDto dto);
        Task<bool> UpdateAsync(int id, UpdateEmployeeDto dto);
        Task<bool> DeleteAsync(int id);
        Task<IEnumerable<EmployeeDto>> GetByDepartmentIdAsync(int departmentId);
    }
}
