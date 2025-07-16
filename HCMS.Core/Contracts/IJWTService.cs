using HCMS.Infrastructure.Entities;

namespace HCMS.Core.Contracts
{
    public interface IJWTService
    {
        string GenerateToken(ApplicationUser user, ICollection<string> roles);
    }
}
