using Newtonsoft.Json;
using ProjectWeb.AdminApp.IServiceBackendAPIs;
using ProjectWeb.Models.SystemUsers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ProjectWeb.AdminApp.Services
{
    public class SystemUserBackendAPI : ISystemUserBackendAPI
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public SystemUserBackendAPI(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        public async Task<string> Authenticate(LoginRequest request)
        {
            var jsonConvert = JsonConvert.SerializeObject(request);
            var httpContext = new StringContent(jsonConvert, Encoding.UTF8, "application/json");
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri("https://localhost:5001");
            var respose = await client.PostAsync("/api/SystemUsers/Login", httpContext);
            var token = await respose.Content.ReadAsStringAsync();
            return token;
        }
    }
}
