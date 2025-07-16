namespace HCMS.Core.DTOs.Employee
{
    public class CreateEmployeeDto
    {
        public string FirstName { get; set; } = default!;
        public string LastName { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string JobTitle { get; set; } = default!;
        public decimal Salary { get; set; }
        public int? DepartmentId { get; set; }
    }
}
