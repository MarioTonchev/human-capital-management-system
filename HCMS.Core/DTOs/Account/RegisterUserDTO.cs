namespace HCMS.Core.DTOs.Account
{
    //TODO: add constraints
    public class RegisterUserDTO
    {
        // User related properties
        public string Username { get; set; } = default!;

        public string Email { get; set; } = default!;

        public string Password { get; set; } = default!;

        public string Role { get; set; } = default!; // Employee, Manager, HRAdmin

        public int EmployeeId { get; set; }

        // Employee (domain-specific) related properties
        public string FirstName { get; set; } = default!;

        public string LastName { get; set; } = default!;

        public string JobTitle { get; set; } = default!;

        public decimal Salary { get; set; }

        public int? DepartmentId { get; set; }
    }
}