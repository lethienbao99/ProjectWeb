using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using ProjectWeb.AdminApp.IServiceBackendAPIs;
using ProjectWeb.Models.AppRoles;
using ProjectWeb.Models.CommonModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace ProjectWeb.AdminApp.Services
{
    public class RoleBackendAPI : IRoleBackendAPI
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public RoleBackendAPI(
            IHttpClientFactory httpClientFactory,
            IConfiguration configuration,
            IHttpContextAccessor httpContextAccessor)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<ResultMessage<List<RoleModel>>> GetAll()
        {
            var Token = _httpContextAccessor.HttpContext.Session.GetString("Token");

            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseURLApi"]);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
            var respose = await client.GetAsync($"/api/Roles/");
            
            var dataRaw = await respose.Content.ReadAsStringAsync();
            if (respose.IsSuccessStatusCode)
            {
                List<RoleModel> myDeserializeObject = (List<RoleModel>)JsonConvert.DeserializeObject(dataRaw, typeof(List<RoleModel>));
                return new ResultObjectSuccess<List<RoleModel>>(myDeserializeObject);
            }
               

            return JsonConvert.DeserializeObject<ResultObjectSuccess<List<RoleModel>>>(dataRaw);
        }
    }
}
