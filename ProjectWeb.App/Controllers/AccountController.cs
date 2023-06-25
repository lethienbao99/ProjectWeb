using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using ProjectWeb.APIServices.IServiceBackendAPIs;
using ProjectWeb.Models.SystemUsers;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using static ProjectWeb.Common.Enums.EnumConstants;

namespace ProjectWeb.EcommerceApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly ISystemUserBackendAPI _systemUserBackendAPI;
        private readonly IConfiguration _config;
        public AccountController(ISystemUserBackendAPI systemUserBackendAPI, IConfiguration config)
        {
            _systemUserBackendAPI = systemUserBackendAPI;
            _config = config;
        }
        [HttpGet]
        public async Task<IActionResult> Login()
        {
            if(User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            if (!ModelState.IsValid)
                return View(ModelState);

            var result = await _systemUserBackendAPI.AuthenticateWithTwoToken(request);
            if (result.Message != "Success")
            {
                ModelState.AddModelError("", result.Message);
                return View();
            }
            var userPrincipal = this.ValidateToken(result.Object.access_token);
            var authProperties = new AuthenticationProperties
            {
                ExpiresUtc = DateTime.UtcNow.AddMinutes(10),
                IsPersistent = false
            };


            HttpContext.Response.Cookies.Append("access_token", result.Object.access_token);
            HttpContext.Response.Cookies.Append("refresh_token", result.Object.refresh_token);

            HttpContext.Session.SetString("access_token", result.Object.access_token);

            HttpContext.Session.SetString(SystemsConstants.SettingLanguage, _config[SystemsConstants.SettingLanguage]);
            await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        userPrincipal,
                        authProperties);
            return RedirectToAction("Index", "Home");

        }

        private ClaimsPrincipal ValidateToken(string jwtToken)
        {
            IdentityModelEventSource.ShowPII = true;

            SecurityToken validatedToken;
            TokenValidationParameters validationParameters = new TokenValidationParameters();

            validationParameters.ValidateLifetime = true;
            validationParameters.ValidateIssuer = false;
            validationParameters.ValidateAudience = false;
            validationParameters.IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Tokens:Key"]));
            ClaimsPrincipal principal = new JwtSecurityTokenHandler().ValidateToken(jwtToken, validationParameters, out validatedToken);
            return principal;
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.Session.Remove("Token");
            HttpContext.Session.Remove("UserID");
            return RedirectToAction("Index", "Home");
        }




        [HttpGet]
        public IActionResult Signup()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Signup(SignUpRequest request)
        {
            if (!ModelState.IsValid)
            {
                return View(request);
            }

            var result = await _systemUserBackendAPI.Signup(request);

            if (result.IsSuccessed)
            {
                return RedirectToAction("Login", "Account");
            }
            ModelState.AddModelError("", result.Message);
            return View(request);
        }

    }
}
