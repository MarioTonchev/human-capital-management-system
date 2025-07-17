using HCMS.Core.DTOs.Employee;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;

namespace HCMS.FrontendAPI.Controllers
{
    [Authorize]
    [Route("api/frontend/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private const string BackendBaseUrl = "http://localhost:5041/api/backend/employees";
        private readonly HttpClient httpClient;

        public EmployeesController(HttpClient httpClient)
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

        [HttpGet("me")]
        public async Task<IActionResult> GetMyProfile()
        {
            ForwardAuthorizationHeaderToBackendApi();

            var response = await httpClient.GetAsync($"{BackendBaseUrl}/me");
            var content = await response.Content.ReadAsStringAsync();

            return StatusCode((int)response.StatusCode, content);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllEmployees()
        {
            ForwardAuthorizationHeaderToBackendApi();

            var response = await httpClient.GetAsync($"{BackendBaseUrl}");
            var content = await response.Content.ReadAsStringAsync();

            return StatusCode((int)response.StatusCode, content);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetEmployeeById(int id)
        {
            ForwardAuthorizationHeaderToBackendApi();

            var response = await httpClient.GetAsync($"{BackendBaseUrl}/{id}");
            var content = await response.Content.ReadAsStringAsync();

            return StatusCode((int)response.StatusCode, content);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEmployee(int id, [FromBody] CreateEmployeeDto dto)
        {
            ForwardAuthorizationHeaderToBackendApi();

            var response = await httpClient.PutAsJsonAsync($"{BackendBaseUrl}/api/employees/{id}", dto);
            var content = await response.Content.ReadAsStringAsync();

            return StatusCode((int)response.StatusCode, content);
        }
    }
}
