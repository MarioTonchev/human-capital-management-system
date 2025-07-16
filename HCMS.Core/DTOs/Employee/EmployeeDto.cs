namespace HCMS.Core.DTOs.Employee
{
    public class EmployeeDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = default!;
        public string LastName { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string JobTitle { get; set; } = default!;
        public decimal Salary { get; set; }
        public int? DepartmentId { get; set; }
    }
}
