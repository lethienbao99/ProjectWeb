using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using ProjectWeb.Models.CommonModels;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using static ProjectWeb.Common.Enums.EnumConstants;

namespace ProjectWeb.APIServices.Services
{
    public class BaseBackendAPI
    {
        public readonly IHttpClientFactory _httpClientFactory;
        public readonly IConfiguration _configuration;
        public readonly IHttpContextAccessor _httpContextAccessor;
        protected BaseBackendAPI(
            IHttpClientFactory httpClientFactory,
            IConfiguration configuration,
            IHttpContextAccessor httpContextAccessor)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// Chỉ trả về data, chứ không return message
        /// </summary>
        /// <param name="Url"></param>
        /// <returns></returns>
        protected async Task<string> GetAsync(string Url)
        {
            var Token = _httpContextAccessor.HttpContext
                .Session
                .GetString(SystemsConstants.Token);

            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration[SystemsConstants.BaseURLApi]);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);

            var response = await client.GetAsync(Url);
            if (response.IsSuccessStatusCode)
            {
                var dataRaw = await response.Content.ReadAsStringAsync();
                return dataRaw;
            }
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Url"></param>
        /// <returns></returns>
        protected async Task<ResultMessage<T>> GetAndReturnAsync<T>(string Url, bool? IsToken = true)
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration[SystemsConstants.BaseURLApi]);

            if (IsToken == true)
            {
                var Token = _httpContextAccessor.HttpContext
                .Session
                .GetString(SystemsConstants.Token);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
            }

            var response = await client.GetAsync(Url);
            var dataRaw = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<ResultObjectError<T>>(dataRaw);
            }
            return JsonConvert.DeserializeObject<ResultObjectSuccess<T>>(dataRaw);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Url"></param>
        /// <returns></returns>
        protected async Task<ResultMessage<T>> DeteleAndReturnAsync<T>(string Url)
        {
            var Token = _httpContextAccessor.HttpContext
                .Session
                .GetString("Token");
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration[SystemsConstants.BaseURLApi]);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
            var response = await client.DeleteAsync(Url);
            var dataRaw = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<ResultObjectError<T>>(dataRaw);
            }
            return JsonConvert.DeserializeObject<ResultObjectSuccess<T>>(dataRaw);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TModelRequest"></typeparam>
        /// <param name="Url"></param>
        /// <param name="request"></param>
        /// <param name="IsToken"></param>
        /// <returns></returns>
        protected async Task<ResultMessage<T>> PostAndReturnAsync<T, TModelRequest>(string Url, TModelRequest request, bool? IsToken)
        {
            var jsonConvert = JsonConvert.SerializeObject(request);
            var httpContext = new StringContent(jsonConvert, Encoding.UTF8, "application/json");
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseURLApi"]);

            if (IsToken == true)
            {
                var Token = _httpContextAccessor.HttpContext.Session.GetString("Token");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
            }

            var response = await client.PostAsync(Url, httpContext);
            var dataRaw = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
                return JsonConvert.DeserializeObject<ResultObjectSuccess<T>>(dataRaw);

            return JsonConvert.DeserializeObject<ResultObjectError<T>>(dataRaw);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TModelRequest"></typeparam>
        /// <param name="Url"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        protected async Task<ResultMessage<T>> PutAndReturnAsync<T, TModelRequest>(string Url, TModelRequest request, bool? IsToken = true)
        {
            var jsonConvert = JsonConvert.SerializeObject(request);
            var httpContext = new StringContent(jsonConvert, Encoding.UTF8, "application/json");
            var client = _httpClientFactory.CreateClient();

            if (IsToken == true)
            {
                var Token = _httpContextAccessor.HttpContext.Session.GetString("Token");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
            }
        
            client.BaseAddress = new Uri(_configuration["BaseURLApi"]);
            var response = await client.PutAsync(Url, httpContext);
            var result = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
                return JsonConvert.DeserializeObject<ResultObjectSuccess<T>>(result);

            return JsonConvert.DeserializeObject<ResultObjectError<T>>(result);
        }


    }
}
