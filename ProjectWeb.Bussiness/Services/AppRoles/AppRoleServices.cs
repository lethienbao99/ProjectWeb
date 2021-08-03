using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ProjectWeb.Common.IServices;
using ProjectWeb.Data.Entities;
using ProjectWeb.Models.AppRoles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectWeb.Bussiness.Services.AppRoles
{
    public class AppRoleServices : IAppRoleServices
    {
        private readonly RoleManager<AppRole> _roleManager;
        public AppRoleServices(RoleManager<AppRole> roleManager)
        {
            _roleManager = roleManager;
        }
        public async Task<List<RoleModel>> GetAll()
        {
            var roles = await _roleManager.Roles.Select(x => 
            new RoleModel()
            { 
                ID = x.Id,
                Name = x.Name,
                Description = x.Description
            }).ToListAsync();
            return roles;
        }
    }
}
