using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ProjectWeb.Common.IServices;
using ProjectWeb.Data.Entities;
using ProjectWeb.Models.CommonModels;
using ProjectWeb.Models.SystemUsers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectWeb.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]

    public class SystemUsersController : ControllerBase
    {
        private readonly ISystemUserServices _systemUserServices;
        private readonly UserManager<SystemUser> _userManager;
        private readonly SignInManager<SystemUser> _signInManager;
        public SystemUsersController(
            ISystemUserServices systemUserServices,
            UserManager<SystemUser> userManager,
            SignInManager<SystemUser> signInManager
            )
        {
            _systemUserServices = systemUserServices;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpPost("Login")]
        [AllowAnonymous]
        public async Task<IActionResult> Authenticate([FromBody] LoginRequest request)
        {
            var user = await _userManager.FindByNameAsync(request.Username);
            if (user == null)
                return BadRequest("User not exit");

            var checkPassword = await _signInManager.PasswordSignInAsync(user, request.Password, false, false);
            if (!checkPassword.Succeeded)
            {
                return BadRequest("Password incorect");
            };

            AuthenticationResult token = await _systemUserServices.GenerateToken(user);
            /*  var obj = new
              {
                  access_token = token.access_token,
                  refresh_token = token.refresh_token
              };*/
            if(token != null)
            {
                var obj = new ResultObjectSuccess<TokenRequest>()
                {
                    IsSuccessed = true,
                    Message = "Success",
                    Object = new TokenRequest()
                    {
                        access_token = token.access_token,
                        refresh_token = token.refresh_token
                    }
                };


                return Ok(obj);
            }
            else
            {
                var obj = new ResultObjectError<TokenRequest>()
                {
                    IsSuccessed = false,
                    Message = "Fail on server!!!"
                };
                return Ok(obj);
            }
            
        }


        [HttpPost("RefreshToken")]
        [AllowAnonymous]
        public async Task<IActionResult> RefreshToken(TokenRequest request)
        {
            var authResponse = await _systemUserServices.RefreshTokenAsync(request.access_token, request.refresh_token);
            return Ok(authResponse);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] SignUpRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _systemUserServices.Register(request);
            if (!result.IsSuccessed)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpGet("Paging")]
        public async Task<IActionResult> GetListUser([FromQuery] UserPagingRequest request)
        {
            var result = await _systemUserServices.GetUserPaging(request);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UserUpdateRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _systemUserServices.Update(id, request);
            if (!result.IsSuccessed)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var result = await _systemUserServices.GetUserByID(id);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _systemUserServices.Delete(id);
            return Ok(result);
        }


        [HttpPut("{id}/role")]
        public async Task<IActionResult> RoleAssign(Guid id, [FromBody] RoleAssignRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _systemUserServices.RoleAssign(id, request);
            if (!result.IsSuccessed)
                return BadRequest(result);

            return Ok(result);
        }
    }
}
