using HCMS.Core.DTOs.Account;
using HCMS.UI.Contracts;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace HCMS.UI.UIServices
{
    public class AccountUIService : IAccountUIService
    {
        private readonly HttpClient client;
        private readonly IHttpContextAccessor httpContextAccessor;

        public AccountUIService(IHttpClientFactory factory, IHttpContextAccessor httpContextAccessor)
        {
            client = factory.CreateClient("BackendApi");
            this.httpContextAccessor = httpContextAccessor;
        }

        public async Task<bool> LoginAsync(LoginDTO dto)
        {
            var response = await client.PostAsJsonAsync("account/login", dto);
            Console.WriteLine(client.BaseAddress);
            if (!response.IsSuccessStatusCode)
            {
                return false;
            }

            var jwt = await response.Content.ReadAsStringAsync();

            httpContextAccessor.HttpContext?.Session.SetString("JWT", jwt);

            return true;
        }

        public Task LogoutAsync()
        {
            httpContextAccessor.HttpContext?.Session.Remove("JWT");

            return Task.CompletedTask;
        }

        public async Task<bool> RegisterAsync(RegisterUserDTO dto)
        {
            AddAuthHeaderIfAvailable();

            var response = await client.PostAsJsonAsync("account/register-user", dto);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            AddAuthHeaderIfAvailable();

            var response = await client.DeleteAsync($"account/{id}");

            return response.IsSuccessStatusCode;
        }

        private void AddAuthHeaderIfAvailable()
        {
            var token = httpContextAccessor.HttpContext?.Session.GetString("JWT");

            if (!string.IsNullOrWhiteSpace(token))
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
        }
    }
}
