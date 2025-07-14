using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HCMS.Infrastructure.Entities
{
    public class Employee
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(50)]
        public string FirstName { get; set; } = default!;

        [Required, MaxLength(50)]
        public string LastName { get; set; } = default!;

        [Required, EmailAddress, MaxLength(100)]
        public string Email { get; set; } = default!;

        [Required, MaxLength(100)]
        public string JobTitle { get; set; } = default!; //maybe make it an enum

        [Required, Column(TypeName = "decimal(18,2)")]
        public decimal Salary { get; set; }

        public int? DepartmentId { get; set; }

        [ForeignKey(nameof(DepartmentId))]
        public Department? Department { get; set; }
    }
}
