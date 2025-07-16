using HCMS.Core.Contracts;
using HCMS.Infrastructure.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace HCMS.BackendAPI.ApiServices
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly UserManager<ApplicationUser> userManager;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor,
                                  UserManager<ApplicationUser> userManager)
        {
            this.httpContextAccessor = httpContextAccessor;
            this.userManager = userManager;
        }

        public async Task<ApplicationUser?> GetCurrentUserAsync()
        {
            var username = httpContextAccessor.HttpContext?.User?.Identity?.Name;

            return await userManager.Users
                .Include(u => u.Employee)
                .ThenInclude(e => e.Department)
                .FirstOrDefaultAsync(u => u.UserName == username);
        }
    }
}
