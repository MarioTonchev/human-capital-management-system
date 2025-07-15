using System.ComponentModel.DataAnnotations;
using static HCMS.Infrastructure.Constants.EntityConstants.DepartmentConstants;

namespace HCMS.Infrastructure.Entities
{
    public class Department
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(DepartmentNameMaxLength)]
        public string Name { get; set; } = default!;

        public ICollection<Employee>? Employees { get; set; } = new List<Employee>();
    }
}