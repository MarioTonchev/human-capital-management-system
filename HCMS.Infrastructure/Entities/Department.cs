using System.ComponentModel.DataAnnotations;

namespace HCMS.Infrastructure.Entities
{
    public class Department
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public string Name { get; set; } = default!;

        public ICollection<Employee>? Employees { get; set; }
    }
}