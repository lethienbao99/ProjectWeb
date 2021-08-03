using ProjectWeb.Common.Repositories;
using ProjectWeb.Data.Entities;
using ProjectWeb.Models.CommonModels;
using ProjectWeb.Models.SystemUsers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectWeb.Common.IServices
{
    public interface ISystemUserServices 
    {
        Task<ResultMessage<string>> Authenticate(LoginRequest request);
        Task<ResultMessage<bool>> Register(SignUpRequest request);
        Task<ResultMessage<PageResultModel<SystemUserModel>>> GetUserPaging(UserPagingRequest request);
        Task<ResultMessage<bool>> Update(Guid ID, UserUpdateRequest request);
        Task<ResultMessage<SystemUserModel>> GetUserByID(Guid ID);
        Task<ResultMessage<bool>> Delete(Guid ID);
        Task<ResultMessage<bool>> RoleAssign(Guid ID, RoleAssignRequest request);

    }

}
