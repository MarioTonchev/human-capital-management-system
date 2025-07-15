using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static HCMS.Infrastructure.Constants.EntityConstants.EmployeeConstants;

namespace HCMS.Infrastructure.Entities
{
    public class Employee
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(EmployeeFirstNameMaxLength)]
        public string FirstName { get; set; } = default!;

        [Required, MaxLength(EmployeeLastNameMaxLength)]
        public string LastName { get; set; } = default!;

        [Required, EmailAddress, MaxLength(EmployeeEmailMaxLength)]
        public string Email { get; set; } = default!;

        [Required, MaxLength(EmployeeJobTitleMaxLength)]
        public string JobTitle { get; set; } = default!;

        [Required, Column(TypeName = "decimal(18,2)")]
        [Range(0, double.MaxValue, ErrorMessage = "Salary must be non-negative")]
        public decimal Salary { get; set; }

        public int? DepartmentId { get; set; }

        [ForeignKey(nameof(DepartmentId))]
        public Department? Department { get; set; }
    }
}
