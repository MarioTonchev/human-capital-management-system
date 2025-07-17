using HCMS.Core.DTOs.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;

namespace HCMS.FrontendAPI.Controllers
{
    [Route("api/frontend/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private const string BackendBaseUrl = "http://localhost:5041/api/backend/account";
        private readonly HttpClient httpClient;

        public AccountController(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        private void ForwardAuthorizationHeaderToBackendApi()
        {
            if (Request.Headers.TryGetValue("Authorization", out var token))
            {
                httpClient.DefaultRequestHeaders.Authorization = AuthenticationHeaderValue.Parse(token);
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO dto)
        {
            var response = await httpClient.PostAsJsonAsync($"{BackendBaseUrl}/login", dto);
            var content = await response.Content.ReadAsStringAsync();

            return StatusCode((int)response.StatusCode, content);
        }

        [Authorize]
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            ForwardAuthorizationHeaderToBackendApi();

            var response = await httpClient.PostAsync($"{BackendBaseUrl}/logout", null);
            var content = await response.Content.ReadAsStringAsync();

            return StatusCode((int)response.StatusCode, content);
        }

        [Authorize]
        [HttpPost("register-user")]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterUserDTO dto)
        {
            ForwardAuthorizationHeaderToBackendApi();

            var response = await httpClient.PostAsJsonAsync($"{BackendBaseUrl}/register-user", dto);
            var content = await response.Content.ReadAsStringAsync();

            return StatusCode((int)response.StatusCode, content);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            ForwardAuthorizationHeaderToBackendApi();

            var response = await httpClient.DeleteAsync($"{BackendBaseUrl}/{id}");
            var content = await response.Content.ReadAsStringAsync();

            return StatusCode((int)response.StatusCode, content);
        }
    }
}
