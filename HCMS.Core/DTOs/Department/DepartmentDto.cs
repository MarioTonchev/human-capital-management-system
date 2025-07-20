using HCMS.Core.DTOs.Employee;

namespace HCMS.Core.DTOs.Department
{
    public class DepartmentDto
    {
        public int Id { get; set; }

        public string Name { get; set; } = default!;

        public List<EmployeeDto>? Employees { get; set; }
    }
}
