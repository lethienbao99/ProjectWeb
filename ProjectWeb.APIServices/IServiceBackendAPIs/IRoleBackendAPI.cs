using ProjectWeb.Models.AppRoles;
using ProjectWeb.Models.CommonModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectWeb.APIServices.IServiceBackendAPIs
{
    public interface IRoleBackendAPI
    {
        Task<ResultMessage<List<RoleModel>>> GetAll();
    }
}
