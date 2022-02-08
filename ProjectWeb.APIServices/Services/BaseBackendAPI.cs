using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using ProjectWeb.Models.CommonModels;
using ProjectWeb.Models.SystemUsers;
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
                /*var Token = _httpContextAccessor.HttpContext
                .Session
                .GetString(SystemsConstants.Token);*/

                var Token = _httpContextAccessor.HttpContext.Request.Cookies["access_token"];

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
            }

            var response = await client.GetAsync(Url);

            if(response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                response = await AccessTokenRefreshWrapper(
                    () => SecuredMethodRequest(Url, null, "GET", null));
            }

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
            var Token = _httpContextAccessor.HttpContext.Request.Cookies["access_token"];
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration[SystemsConstants.BaseURLApi]);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
            var response = await client.DeleteAsync(Url);

            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                response = await AccessTokenRefreshWrapper(
                    () => SecuredMethodRequest(Url, null, "DELETE", null));
            }

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
            var httpContent = new StringContent(jsonConvert, Encoding.UTF8, "application/json");
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration[SystemsConstants.BaseURLApi]);

            if (IsToken == true)
            {
                var Token = _httpContextAccessor.HttpContext.Request.Cookies["access_token"];
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
            }

            var response = await client.PostAsync(Url, httpContent);

            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                response = await AccessTokenRefreshWrapper(
                    () => SecuredMethodRequest(Url, httpContent, "POST", null));
            }

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
                var Token = _httpContextAccessor.HttpContext.Request.Cookies["access_token"];
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
            }
        
            client.BaseAddress = new Uri(_configuration[SystemsConstants.BaseURLApi]);
            var response = await client.PutAsync(Url, httpContext);

            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                response = await AccessTokenRefreshWrapper(
                    () => SecuredMethodRequest(Url, httpContext, "PUT", null));
            }

            var result = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
                return JsonConvert.DeserializeObject<ResultObjectSuccess<T>>(result);

            return JsonConvert.DeserializeObject<ResultObjectError<T>>(result);
        }

        public async Task<HttpResponseMessage> AccessTokenRefreshWrapper(Func<Task<HttpResponseMessage>> initialRequest)
        {
            var response = await initialRequest();

            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                var tokenObject = new TokenRequest();
                tokenObject.access_token = _httpContextAccessor.HttpContext.Request.Cookies["access_token"];
                tokenObject.refresh_token = _httpContextAccessor.HttpContext.Request.Cookies["refresh_token"];

                //Kiểm tra còn token cũ thì xóa đi trong session.
                if (tokenObject.access_token != null)
                    _httpContextAccessor.HttpContext.Session.Remove("access_token");

                var jsonConvert = JsonConvert.SerializeObject(tokenObject);

                var httpContext = new StringContent(jsonConvert, Encoding.UTF8, "application/json");

                var client = _httpClientFactory.CreateClient();
                client.BaseAddress = new Uri(_configuration[SystemsConstants.BaseURLApi]);

                var responseToken = await client.PostAsync("/api/SystemUsers/RefreshToken", httpContext);
                var dataRaw = await responseToken.Content.ReadAsStringAsync();
                var dataObject = JsonConvert.DeserializeObject<TokenRequest>(dataRaw);

                //Set lại giá trị token mới trong session
                if (dataObject.access_token != null)
                    _httpContextAccessor.HttpContext.Session.SetString("access_token", dataObject.access_token);

                response = await initialRequest();
            }

            return response;

        }

        public async Task<HttpResponseMessage> SecuredMethodRequest(string url, StringContent content, string method, MultipartFormDataContent multiContent)
        {
            var token = _httpContextAccessor.HttpContext.Session.GetString("access_token");
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration[SystemsConstants.BaseURLApi]);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            if (multiContent != null)
            { 
                if(method == "POST")
                    return await client.PostAsync(url, multiContent);
                else if (method == "PUT")
                    return await client.PutAsync(url, multiContent);
            }

            if (method == "GET")
                return await client.GetAsync(url);
            else if (method == "POST")
                return await client.PostAsync(url, content);
            else if (method == "POST" && multiContent != null)
                return await client.PostAsync(url, content);
            else if (method == "PUT")
                return await client.PutAsync(url, content);
            else if (method == "DELETE")
                return await client.DeleteAsync(url);
            else
                return null;
        }

    }
}
