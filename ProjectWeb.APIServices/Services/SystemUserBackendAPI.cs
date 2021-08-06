using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using ProjectWeb.APIServices.IServiceBackendAPIs;
using ProjectWeb.Models.CommonModels;
using ProjectWeb.Models.SystemUsers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ProjectWeb.APIServices.Services
{
    public class SystemUserBackendAPI : BaseBackendAPI, ISystemUserBackendAPI
    {
        public SystemUserBackendAPI(
            IHttpClientFactory httpClientFactory,
            IConfiguration configuration,
            IHttpContextAccessor httpContextAccessor) : base(httpClientFactory, configuration, httpContextAccessor)
        {

        }
        public async Task<ResultMessage<string>> Authenticate(LoginRequest request)
        {
            return await PostAndReturnAsync<string, LoginRequest>("/api/SystemUsers/Login", request, false);
        }

        public async Task<ResultMessage<SystemUserModel>> GetUserByID(Guid ID)
        {
            return await GetAndReturnAsync<SystemUserModel>($"/api/SystemUsers/{ID}");
        }

        public async Task<ResultMessage<bool>> Delete(Guid ID)
        {
            return await DeteleAndReturnAsync<bool>($"/api/SystemUsers/{ID}");
        }

        public async Task<ResultMessage<PageResultModel<SystemUserModel>>> GetUserPaging(UserPagingRequest request)
        {
            return await GetAndReturnAsync<PageResultModel<SystemUserModel>>($"/api/SystemUsers/Paging?pageIndex=" +
                $"{request.PageIndex}&pageSize={request.PageSize}&keyword={request.Keyword}");
        }

        public async Task<ResultMessage<bool>> Signup(SignUpRequest request)
        {
            return await PostAndReturnAsync<bool, SignUpRequest>("/api/SystemUsers", request, true);
        }

        public async Task<ResultMessage<bool>> Update(Guid ID, UserUpdateRequest request)
        {
            return await PutAndReturnAsync<bool, UserUpdateRequest>($"/api/SystemUsers/{ID}", request);
        }

        public async Task<ResultMessage<bool>> RoleAssign(Guid ID, RoleAssignRequest request)
        {
            return await PutAndReturnAsync<bool, RoleAssignRequest>($"/api/SystemUsers/{ID}/role", request);
        }
    }
}
