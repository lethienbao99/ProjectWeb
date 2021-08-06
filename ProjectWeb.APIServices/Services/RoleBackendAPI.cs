using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using ProjectWeb.APIServices.IServiceBackendAPIs;
using ProjectWeb.Models.AppRoles;
using ProjectWeb.Models.CommonModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace ProjectWeb.APIServices.Services
{
    public class RoleBackendAPI : BaseBackendAPI, IRoleBackendAPI
    {
        public RoleBackendAPI(
            IHttpClientFactory httpClientFactory,
            IConfiguration configuration,
            IHttpContextAccessor httpContextAccessor) : base(httpClientFactory, configuration, httpContextAccessor)
        { }

        public async Task<ResultMessage<List<RoleModel>>> GetAll()
        {
            var dataRaw = await GetAsync("/api/Roles/");
            if (dataRaw != null)
            {
                List<RoleModel> myDeserializeObject = (List<RoleModel>)JsonConvert.DeserializeObject(dataRaw, typeof(List<RoleModel>));
                return new ResultObjectSuccess<List<RoleModel>>(myDeserializeObject);
            }
            return null;
        }
    }
}
