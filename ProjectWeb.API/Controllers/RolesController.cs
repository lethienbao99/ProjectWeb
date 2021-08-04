using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ProjectWeb.Common.IServices;
using ProjectWeb.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectWeb.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]

    public class RolesController : ControllerBase
    {
        private readonly IAppRoleServices _appRoleServices;
        public RolesController(IAppRoleServices appRoleServices)
        {
            _appRoleServices = appRoleServices;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var roles = await _appRoleServices.GetAll();
            return Ok(roles);
        }
    }
}
