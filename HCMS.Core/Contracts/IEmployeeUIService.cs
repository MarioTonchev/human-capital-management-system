using HCMS.Core.DTOs.Employee;

namespace HCMS.Core.Contracts
{
    public interface IEmployeeUIService
    {
        Task<EmployeeDto> GetMyProfileAsync();
        Task<List<EmployeeDto>> GetAllEmployeesAsync();
        Task<EmployeeDto?> GetEmployeeByIdAsync(int id);
        Task<bool> UpdateEmployeeAsync(int id, UpdateEmployeeDto dto);
    }
}
