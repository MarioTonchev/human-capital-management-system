using System.ComponentModel.DataAnnotations;
using static HCMS.Infrastructure.Constants.EntityConstants.EmployeeConstants;
using static HCMS.Infrastructure.Constants.EntityConstants.ApplicationUserConstants;

namespace HCMS.Core.DTOs.Account
{
    public class RegisterUserDTO
    {
        // User identity related properties
        [Required]
        [MaxLength(UsernameMaxLength)]
        public string Username { get; set; } = default!;

        [Required]
        [MaxLength(PasswordMaxLength)]
        public string Password { get; set; } = default!;

        [Required]
        public string Role { get; set; } = default!; // Employee, Manager, HRAdmin

        // Employee (domain-specific) related properties
        [Required(ErrorMessage = $"{nameof(FirstName)} is required")]
        [MaxLength(EmployeeFirstNameMaxLength)]
        public string FirstName { get; set; } = default!;

        [Required(ErrorMessage = $"{nameof(LastName)} is required")]
        [MaxLength(EmployeeLastNameMaxLength)]
        public string LastName { get; set; } = default!;

        [Required(ErrorMessage = $"{nameof(Email)} is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        [MaxLength(EmployeeEmailMaxLength)]
        public string Email { get; set; } = default!;

        [Required(ErrorMessage = $"{nameof(JobTitle)} is required")]
        [MaxLength(EmployeeJobTitleMaxLength)]
        public string JobTitle { get; set; } = default!;

        [Required(ErrorMessage = $"{nameof(Salary)} is required")]
        [Range(0, double.MaxValue, ErrorMessage = "Salary must be a non-negative number")]
        public decimal Salary { get; set; }

        public int? DepartmentId { get; set; }
    }
}