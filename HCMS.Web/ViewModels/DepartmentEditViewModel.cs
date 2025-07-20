using System.ComponentModel.DataAnnotations;
using static HCMS.Infrastructure.Constants.EntityConstants.DepartmentConstants;

namespace HCMS.UI.ViewModels
{
    public class DepartmentEditViewModel
    {
        [Required]
        public int Id { get; set; }

        [Required(ErrorMessage = $"{nameof(Name)} is required")]
        [MaxLength(DepartmentNameMaxLength)]
        public string Name{ get; set; } = default!;
    }
}
