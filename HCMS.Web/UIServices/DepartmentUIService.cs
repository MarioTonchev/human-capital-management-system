using HCMS.Core.DTOs.Department;
using HCMS.UI.Contracts;
using System.Net.Http.Headers;

namespace HCMS.UI.UIServices
{
    public class DepartmentUIService : IDepartmentUIService
    {
        private readonly IHttpClientFactory httpClientFactory;
        private readonly IHttpContextAccessor httpContextAccessor;

        public DepartmentUIService(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
        {
            this.httpClientFactory = httpClientFactory;
            this.httpContextAccessor = httpContextAccessor;
        }

        public async Task<List<DepartmentDto>> GetAllDepartmentsAsync()
        {
            var client = CreateClientWithToken();
            var response = await client.GetAsync("departments");

            if (!response.IsSuccessStatusCode)
            {
                return new List<DepartmentDto>();
            }

            return await response.Content.ReadFromJsonAsync<List<DepartmentDto>>() ?? new List<DepartmentDto>();
        }

        public async Task<DepartmentDto> GetDepartmentByIdAsync(int id)
        {
            var client = CreateClientWithToken();
            var response = await client.GetAsync($"departments/{id}");

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            return await response.Content.ReadFromJsonAsync<DepartmentDto>();
        }

        public async Task<bool> CreateDepartmentAsync(CreateDepartmentDto dto)
        {
            var client = CreateClientWithToken();
            var response = await client.PostAsJsonAsync("departments", dto);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateDepartmentAsync(int id, UpdateDepartmentDto dto)
        {
            var client = CreateClientWithToken();
            var response = await client.PutAsJsonAsync($"departments/{id}", dto);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteDepartmentAsync(int id)
        {
            var client = CreateClientWithToken();
            var response = await client.DeleteAsync($"departments/{id}");

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
