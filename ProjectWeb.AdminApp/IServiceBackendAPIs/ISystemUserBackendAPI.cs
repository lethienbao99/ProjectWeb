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
        Task<ResultMessage<string>> Authenticate(LoginRequest request);
        Task<ResultMessage<PageResultModel<SystemUserModel>>> GetUserPaging(UserPagingRequest request);
        Task<ResultMessage<bool>> Signup(SignUpRequest request);
        Task<ResultMessage<bool>> Update(Guid ID, UserUpdateRequest request);
        Task<ResultMessage<SystemUserModel>> GetUserByID(Guid ID);
        Task<ResultMessage<bool>> Delete(Guid ID);



    }
}
