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
        Task<string> Authenticate(LoginRequest request);
        Task<bool> Register(SignUpRequest request);
        Task<PageResultModel<SystemUserModel>> GetUserPaging(UserPagingRequest request);

    }

}
