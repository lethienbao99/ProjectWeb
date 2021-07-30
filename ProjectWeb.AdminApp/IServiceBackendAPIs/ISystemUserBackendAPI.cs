using ProjectWeb.Models.CommonModels;
using ProjectWeb.Models.SystemUsers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectWeb.AdminApp.IServiceBackendAPIs
{
    public interface ISystemUserBackendAPI
    {
        Task<string> Authenticate(LoginRequest request);
        Task<PageResultModel<SystemUserModel>> GetUserPaging(UserPagingRequest request);
        Task<bool> Signup(SignUpRequest request);

    }
}
