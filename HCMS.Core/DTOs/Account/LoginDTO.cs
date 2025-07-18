using System.ComponentModel.DataAnnotations;
using static HCMS.Infrastructure.Constants.EntityConstants.ApplicationUserConstants;

namespace HCMS.Core.DTOs.Account
{
    public class LoginDTO
    {
        [Required]
        [MaxLength(UsernameMaxLength)]
        public string Username { get; set; } = default!;

        [Required]
        [MaxLength(PasswordMaxLength)]
        public string Password { get; set; } = default!;
    }
}
