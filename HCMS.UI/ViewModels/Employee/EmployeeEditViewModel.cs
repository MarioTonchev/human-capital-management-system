using System.ComponentModel.DataAnnotations;
using static HCMS.Infrastructure.Constants.EntityConstants.EmployeeConstants;

namespace HCMS.UI.ViewModels.Employee
{
    public class EmployeeEditViewModel
    {
        [Required]
        public int Id { get; set; }

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
