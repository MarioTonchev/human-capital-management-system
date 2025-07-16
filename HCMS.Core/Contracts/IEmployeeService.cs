using HCMS.Core.DTOs.Employee;

namespace HCMS.Core.Contracts
{
    public interface IEmployeeService
    {
        Task<IEnumerable<EmployeeDto>> GetAllAsync();
        Task<EmployeeDto?> GetByIdAsync(int id);
        Task CreateAsync(CreateEmployeeDto dto);
        Task UpdateAsync(int id, CreateEmployeeDto dto);
        Task DeleteAsync(int id);
        Task<IEnumerable<EmployeeDto>> GetByDepartmentIdAsync(int departmentId);
        Task<IEnumerable<EmployeeDto>> GetByDepartmentNameAsync(string departmentName);
    }
}
