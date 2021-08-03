﻿using ProjectWeb.Models.AppRoles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectWeb.Common.IServices
{
    public interface IAppRoleServices
    {
        Task<List<RoleModel>> GetAll();
    }
}
