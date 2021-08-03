using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using ProjectWeb.AdminApp.IServiceBackendAPIs;
using ProjectWeb.Models.CommonModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using static ProjectWeb.Common.Enums.EnumConstants;

namespace ProjectWeb.AdminApp.Services
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

            var respose = await client.GetAsync(Url);
            if (respose.IsSuccessStatusCode)
            {
                var dataRaw = await respose.Content.ReadAsStringAsync();
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
        protected async Task<ResultMessage<T>> GetAndReturnAsync<T>(string Url)
        {
            var Token = _httpContextAccessor.HttpContext
                .Session
                .GetString(SystemsConstants.Token);

            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration[SystemsConstants.BaseURLApi]);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
            
            var respose = await client.GetAsync(Url);
            var dataRaw = await respose.Content.ReadAsStringAsync();
            if (!respose.IsSuccessStatusCode)
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
            var respose = await client.DeleteAsync(Url);
            var dataRaw = await respose.Content.ReadAsStringAsync();
            if (!respose.IsSuccessStatusCode)
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

            if(IsToken == true)
            {
                var Token = _httpContextAccessor.HttpContext.Session.GetString("Token");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
            }

            var respose = await client.PostAsync(Url, httpContext);
            var dataRaw = await respose.Content.ReadAsStringAsync();
            if (respose.IsSuccessStatusCode)
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
        protected async Task<ResultMessage<T>> PutAndReturnAsync<T, TModelRequest>(string Url, TModelRequest request)
        {
            var jsonConvert = JsonConvert.SerializeObject(request);
            var httpContext = new StringContent(jsonConvert, Encoding.UTF8, "application/json");

            var Token = _httpContextAccessor.HttpContext.Session.GetString("Token");
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseURLApi"]);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);

            var respose = await client.PutAsync(Url, httpContext);
            var result = await respose.Content.ReadAsStringAsync();
            if (respose.IsSuccessStatusCode)
                return JsonConvert.DeserializeObject<ResultObjectSuccess<T>>(result);

            return JsonConvert.DeserializeObject<ResultObjectError<T>>(result);
        }


    }
}
