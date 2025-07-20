using HCMS.Core.DTOs.Account;

namespace HCMS.UI.Contracts
{
    public interface IAccountUIService
    {
        Task<bool> LoginAsync(LoginDTO loginDto);
        Task<bool> RegisterAsync(RegisterUserDTO registerDto);
        Task LogoutAsync();
        Task<bool> DeleteUserAsync(int id);
    }
}
