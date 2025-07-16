using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace HCMS.Infrastructure.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public int? EmployeeId { get; set; }
            
        [ForeignKey(nameof(EmployeeId))]
        public Employee? Employee { get; set; }
    }
}
