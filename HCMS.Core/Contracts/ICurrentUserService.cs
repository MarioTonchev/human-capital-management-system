using HCMS.Infrastructure.Entities;

namespace HCMS.Core.Contracts
{
    public interface ICurrentUserService
    {
        Task<ApplicationUser?> GetCurrentUserAsync();
    }
}
