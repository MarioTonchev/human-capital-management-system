using HCMS.Core.DTOs.Employee;
using HCMS.UI.Contracts;
using System.Net.Http;
using System.Net.Http.Headers;

namespace HCMS.UI.UIServices
{
    public class EmployeeUIService : IEmployeeUIService
    {
        private readonly IHttpClientFactory httpClientFactory;
        private readonly IHttpContextAccessor httpContextAccessor;

        public EmployeeUIService(IHttpClientFactory factory, IHttpContextAccessor httpContextAccessor)
        {
            this.httpClientFactory = factory;
            this.httpContextAccessor = httpContextAccessor;
        }

        public async Task<EmployeeDto> GetMyProfileAsync()
        {
            var client = CreateClientWithToken();
            var response = await client.GetAsync("employees/me");

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            return await response.Content.ReadFromJsonAsync<EmployeeDto>();
        }

        public async Task<List<EmployeeDto>> GetAllEmployeesAsync()
        {
            var client = CreateClientWithToken();

            var response = await client.GetAsync("employees");

            if (!response.IsSuccessStatusCode)
            {
                return new List<EmployeeDto>();
            }

            var employees = await response.Content.ReadFromJsonAsync<List<EmployeeDto>>();
            return employees ?? new List<EmployeeDto>();
        }

        public async Task<EmployeeDto> GetEmployeeByIdAsync(int id)
        {
            var client = CreateClientWithToken();
            
            var response = await client.GetAsync($"employees/{id}");

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            return await response.Content.ReadFromJsonAsync<EmployeeDto>();
        }

        public async Task<bool> UpdateEmployeeAsync(int id, UpdateEmployeeDto dto)
        {
            var client = CreateClientWithToken();

            var response = await client.PutAsJsonAsync($"employees/{id}", dto);

            return response.IsSuccessStatusCode;
        }

        //Note: This method is used to create an HttpClient with the JWT token from the session.
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
