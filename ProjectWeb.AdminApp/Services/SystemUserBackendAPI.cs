using Microsoft.AspNetCore.Http;
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
        private readonly IHttpContextAccessor _httpContextAccessor;
        public SystemUserBackendAPI(
            IHttpClientFactory httpClientFactory, 
            IConfiguration configuration,
            IHttpContextAccessor httpContextAccessor)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<ResultMessage<string>> Authenticate(LoginRequest request)
        {
            var jsonConvert = JsonConvert.SerializeObject(request);
            var httpContext = new StringContent(jsonConvert, Encoding.UTF8, "application/json");
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseURLApi"]);
            var respose = await client.PostAsync("/api/SystemUsers/Login", httpContext);

            var dataRaw = await respose.Content.ReadAsStringAsync();

            if (respose.IsSuccessStatusCode)
                return JsonConvert.DeserializeObject<ResultObjectSuccess<string>>(dataRaw);

            return new ResultObjectError<string>(dataRaw); 
        }


        public async Task<ResultMessage<SystemUserModel>> GetUserByID(Guid ID)
        {
            var Token = _httpContextAccessor.HttpContext.Session.GetString("Token");
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseURLApi"]);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
            var respose = await client.GetAsync($"/api/SystemUsers/{ID}");
            var dataRaw = await respose.Content.ReadAsStringAsync();
            if (!respose.IsSuccessStatusCode)
                return JsonConvert.DeserializeObject<ResultObjectError<SystemUserModel>>(dataRaw);

            var user = JsonConvert.DeserializeObject<ResultObjectSuccess<SystemUserModel>>(dataRaw);
            return user;
        }

        public async Task<ResultMessage<bool>> Delete(Guid ID)
        {
            var Token = _httpContextAccessor.HttpContext.Session.GetString("Token");
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseURLApi"]);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
            var respose = await client.DeleteAsync($"/api/SystemUsers/{ID}");
            var dataRaw = await respose.Content.ReadAsStringAsync();
            if (!respose.IsSuccessStatusCode)
                return JsonConvert.DeserializeObject<ResultObjectError<bool>>(dataRaw);

            var result = JsonConvert.DeserializeObject<ResultObjectSuccess<bool>>(dataRaw);
            return result;
        }

        public async Task<ResultMessage<PageResultModel<SystemUserModel>>> GetUserPaging(UserPagingRequest request)
        {
            var Token = _httpContextAccessor.HttpContext.Session.GetString("Token");

            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseURLApi"]);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
            var respose = await client.GetAsync($"/api/SystemUsers/Paging?pageIndex=" + 
                $"{request.PageIndex}&pageSize={request.PageSize}&keyword={request.Keyword}");

            var dataRaw = await respose.Content.ReadAsStringAsync();

            var users = JsonConvert.DeserializeObject<ResultObjectSuccess<PageResultModel<SystemUserModel>>>(dataRaw);
            return users;
        } 

        public async Task<ResultMessage<bool>> Signup(SignUpRequest request)
        {
            var jsonConvert = JsonConvert.SerializeObject(request);
            var httpContext = new StringContent(jsonConvert, Encoding.UTF8, "application/json");

            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseURLApi"]);

            var respose = await client.PostAsync("/api/SystemUsers", httpContext);
            var result = await respose.Content.ReadAsStringAsync();
            if(respose.IsSuccessStatusCode)
                return JsonConvert.DeserializeObject<ResultObjectSuccess<bool>>(result);

            return JsonConvert.DeserializeObject<ResultObjectError<bool>>(result);

        }

        public async Task<ResultMessage<bool>> Update(Guid ID, UserUpdateRequest request)
        {
            var Token = _httpContextAccessor.HttpContext.Session.GetString("Token");
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseURLApi"]);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);

            var jsonConvert = JsonConvert.SerializeObject(request);
            var httpContext = new StringContent(jsonConvert, Encoding.UTF8, "application/json");

            var respose = await client.PutAsync($"/api/SystemUsers/{ID}", httpContext);
            var result = await respose.Content.ReadAsStringAsync();
            if (respose.IsSuccessStatusCode)
                return JsonConvert.DeserializeObject<ResultObjectSuccess<bool>>(result);

            return JsonConvert.DeserializeObject<ResultObjectError<bool>>(result);
        }

        public async Task<ResultMessage<bool>> RoleAssign(Guid ID, RoleAssignRequest request)
        {
            var Token = _httpContextAccessor.HttpContext.Session.GetString("Token");
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseURLApi"]);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);

            var jsonConvert = JsonConvert.SerializeObject(request);
            var httpContext = new StringContent(jsonConvert, Encoding.UTF8, "application/json");

            var respose = await client.PutAsync($"/api/SystemUsers/{ID}/role", httpContext);
            var result = await respose.Content.ReadAsStringAsync();
            if (respose.IsSuccessStatusCode)
                return JsonConvert.DeserializeObject<ResultObjectSuccess<bool>>(result);

            return JsonConvert.DeserializeObject<ResultObjectError<bool>>(result);
        }
    }
}
