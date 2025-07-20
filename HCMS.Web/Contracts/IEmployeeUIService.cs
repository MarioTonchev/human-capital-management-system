using HCMS.Core.DTOs.Employee;

namespace HCMS.UI.Contracts
{
    public interface IEmployeeUIService
    {
        Task<EmployeeDto> GetMyProfileAsync();
        Task<List<EmployeeDto>> GetAllEmployeesAsync();
        Task<EmployeeDto?> GetEmployeeByIdAsync(int id);
        Task<bool> UpdateEmployeeAsync(int id, UpdateEmployeeDto dto);
    }
}
