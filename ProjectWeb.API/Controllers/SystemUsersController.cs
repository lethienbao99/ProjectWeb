using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjectWeb.Common.IServices;
using ProjectWeb.Models.SystemUsers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectWeb.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class SystemUsersController : ControllerBase
    {
        private readonly ISystemUserServices _systemUserServices;
        public SystemUsersController(ISystemUserServices systemUserServices)
        {
            _systemUserServices = systemUserServices;
        }

        [HttpPost("Login")]
        [AllowAnonymous]
        public async Task<IActionResult> Authenticate([FromBody] LoginRequest request)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            var resultToken = await _systemUserServices.Authenticate(request);
            if(string.IsNullOrEmpty(resultToken))
            {
                return BadRequest("Username or Password is incorrect");
            }
            return Ok(resultToken);
        }

        [HttpPost("Signup")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] SignUpRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _systemUserServices.Register(request);
            if (!result)
            {
                return BadRequest("Fail");
            }
            return Ok();
        }
    }
}
