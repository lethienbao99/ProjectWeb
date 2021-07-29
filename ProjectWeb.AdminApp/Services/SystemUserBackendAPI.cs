using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using ProjectWeb.AdminApp.IServiceBackendAPIs;
using ProjectWeb.Models.CommonModels;
using ProjectWeb.Models.SystemUsers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ProjectWeb.AdminApp.Services
{
    public class SystemUserBackendAPI : ISystemUserBackendAPI
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        public SystemUserBackendAPI(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }
        public async Task<string> Authenticate(LoginRequest request)
        {
            var jsonConvert = JsonConvert.SerializeObject(request);
            var httpContext = new StringContent(jsonConvert, Encoding.UTF8, "application/json");
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseURLApi"]);
            var respose = await client.PostAsync("/api/SystemUsers/Login", httpContext);
            var token = await respose.Content.ReadAsStringAsync();
            return token;
        }

        public async Task<PageResultModel<SystemUserModel>> GetUserPaging(UserPagingRequest request)
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseURLApi"]);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", request.BearerToken);
            var respose = await client.GetAsync($"/api/SystemUsers/Paging?pageIndex=" + 
                $"{request.PageIndex}&pageSize={request.PageSize}&keyword={request.Keyword}");

            var dataRaw = await respose.Content.ReadAsStringAsync();

            var users = JsonConvert.DeserializeObject<PageResultModel<SystemUserModel>>(dataRaw);
            return users;
        }
    }
}
