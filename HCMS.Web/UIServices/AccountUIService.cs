using HCMS.Core.DTOs.Account;
using HCMS.UI.Contracts;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace HCMS.UI.UIServices
{
    public class AccountUIService : IAccountUIService
    {
        private readonly IHttpClientFactory httpClientFactory;
        private readonly IHttpContextAccessor httpContextAccessor;

        public AccountUIService(IHttpClientFactory factory, IHttpContextAccessor httpContextAccessor)
        {
            httpClientFactory = factory;
            this.httpContextAccessor = httpContextAccessor;
        }

        public async Task<bool> LoginAsync(LoginDTO dto)
        {
            var client = httpClientFactory.CreateClient("BackendApi");
            var response = await client.PostAsJsonAsync("account/login", dto);

            if (!response.IsSuccessStatusCode)
            {
                return false;
            }

            var jwt = await response.Content.ReadAsStringAsync();   
            jwt = jwt.Trim('"');

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
            var client = CreateClientWithToken();

            var response = await client.PostAsJsonAsync("account/register-user", dto);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            var client = CreateClientWithToken();

            var response = await client.DeleteAsync($"account/{id}");

            return response.IsSuccessStatusCode;
        }

        private HttpClient CreateClientWithToken()
        {
            var client = httpClientFactory.CreateClient("BackendApi");
            var token = httpContextAccessor.HttpContext?.Session.GetString("JWT");

            if (!string.IsNullOrWhiteSpace(token))
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            return client;
        }
    }
}
