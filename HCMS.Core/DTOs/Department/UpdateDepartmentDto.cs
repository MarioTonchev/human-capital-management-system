using System.ComponentModel.DataAnnotations;
using static HCMS.Infrastructure.Constants.EntityConstants.DepartmentConstants;


namespace HCMS.Core.DTOs.Department
{
    public class UpdateDepartmentDto
    {
        [Required(ErrorMessage = $"{nameof(Name)} is required")]
        [MaxLength(DepartmentNameMaxLength)]
        public string Name { get; set; } = default!;
    }
}
