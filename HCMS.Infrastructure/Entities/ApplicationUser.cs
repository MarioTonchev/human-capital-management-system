using Microsoft.AspNetCore.Identity;

namespace HCMS.Infrastructure.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public int? EmployeeId { get; set; }
        public Employee? Employee { get; set; }
    }
}
