using HCMS.Core.DTOs.Department;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;

namespace HCMS.FrontendAPI.Controllers
{
    [Authorize]
    [Route("api/frontend/[controller]")]
    [ApiController]
    public class DepartmentsController : ControllerBase
    {
        private const string BackendBaseUrl = "http://localhost:5041/api/backend/departments";
        private readonly HttpClient httpClient;

        public DepartmentsController(HttpClient httpClient)
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

        [HttpGet]
        public async Task<IActionResult> GetAllDepartments()
        {
            ForwardAuthorizationHeaderToBackendApi();

            var response = await httpClient.GetAsync($"{BackendBaseUrl}");
            var content = await response.Content.ReadAsStringAsync();

            return StatusCode((int)response.StatusCode, content);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDepartmentById(int id)
        {
            ForwardAuthorizationHeaderToBackendApi();

            var response = await httpClient.GetAsync($"{BackendBaseUrl}/{id}");
            var content = await response.Content.ReadAsStringAsync();

            return StatusCode((int)response.StatusCode, content);
        }

        [HttpPost]
        public async Task<IActionResult> CreateDepartment([FromBody] CreateDepartmentDto dto)
        {
            ForwardAuthorizationHeaderToBackendApi();

            var response = await httpClient.PostAsJsonAsync($"{BackendBaseUrl}", dto);
            var content = await response.Content.ReadAsStringAsync();

            return StatusCode((int)response.StatusCode, content);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDepartment(int id, [FromBody] UpdateDepartmentDto dto)
        {
            ForwardAuthorizationHeaderToBackendApi();

            var response = await httpClient.PutAsJsonAsync($"{BackendBaseUrl}/{id}", dto);
            var content = await response.Content.ReadAsStringAsync();

            return StatusCode((int)response.StatusCode, content);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDepartment(int id)
        {
            ForwardAuthorizationHeaderToBackendApi();
            
            var response = await httpClient.DeleteAsync($"{BackendBaseUrl}/{id}");
            var content = await response.Content.ReadAsStringAsync();

            return StatusCode((int)response.StatusCode, content);
        }
    }
}
