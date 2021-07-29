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
    }
}
