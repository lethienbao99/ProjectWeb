using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using ProjectWeb.AdminApp.IServiceBackendAPIs;
using ProjectWeb.Models.SystemUsers;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ProjectWeb.AdminApp.Controllers
{

    public class SystemUserController : Controller
    {
        private readonly ISystemUserBackendAPI _systemUserBackendAPI;
        private readonly IConfiguration _config;
        public SystemUserController(ISystemUserBackendAPI systemUserBackendAPI, IConfiguration config)
        {
            _systemUserBackendAPI = systemUserBackendAPI;
            _config = config;
        }
        public async Task<IActionResult> Index(string keyword, int pageIndex = 1, int pageSize = 5)
        {
            var TokenInSession = HttpContext.Session.GetString("Token");
            if(TokenInSession == null)
                return RedirectToAction("Login", "SystemUser");

            var request = new UserPagingRequest()
            {
                Keyword = keyword,
                PageIndex = pageIndex,
                PageSize = pageSize
            };
            ViewBag.Keyword = keyword;
            if (TempData["SuccessMessage"] != null)
            {
                ViewBag.SuccessMessage = TempData["SuccessMessage"];
            }
            if (TempData["ErrorMessage"] != null)
            {
                ViewBag.ErrorMessage = TempData["ErrorMessage"];
            }
            var data = await _systemUserBackendAPI.GetUserPaging(request);
            return View(data.Object);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(SignUpRequest request)
        {
            var ErrorString = "";
            if (!ModelState.IsValid)
            {
                foreach (var item in ModelState.Root.Errors)
                {
                    ErrorString += item.ErrorMessage;
                }
                if (ErrorString == "")
                {
                    foreach (var item in ModelState.Root.Children)
                    {
                        if (item.Errors.Count > 0)
                        {
                            ErrorString += item.Errors[0].ErrorMessage;
                        }
                    }
                }
                TempData["ErrorMessage"] = ErrorString;
                if (TempData["ErrorMessage"] != null)
                {
                    ViewBag.ErrorMessage = TempData["ErrorMessage"];
                }
                return View();
            }

            var result = await _systemUserBackendAPI.Signup(request);
            if (result.IsSuccessed)
            {
                TempData["SuccessMessage"] = "Thêm mới thành công";
                return RedirectToAction("Index", "SystemUser");
            }
               
            ModelState.AddModelError("", result.Message);
            TempData["ErrorMessage"] = result.Message;
            if(TempData["ErrorMessage"] != null)
            {
                ViewBag.ErrorMessage = TempData["ErrorMessage"];
            }
            return View(request);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(UserUpdateRequest request)
        {
            var ErrorString = "";
            if (!ModelState.IsValid)
            {
                foreach (var item in ModelState.Root.Errors)
                {
                    ErrorString += item.ErrorMessage;
                }
                if(ErrorString == "")
                {
                    foreach (var item in ModelState.Root.Children)
                    {
                        if(item.Errors.Count > 0)
                        {
                            ErrorString += item.Errors[0].ErrorMessage;
                        }
                    }
                }
                TempData["ErrorMessage"] = ErrorString;
                if (TempData["ErrorMessage"] != null)
                {
                    ViewBag.ErrorMessage = TempData["ErrorMessage"];
                }
                return View();
            }

            var result = await _systemUserBackendAPI.Update(request.ID, request);
            if (result.IsSuccessed)
            {
                TempData["SuccessMessage"] = "Cập nhật thành công";
                return RedirectToAction("Index", "SystemUser");
            }
            ModelState.AddModelError("", result.Message);
            TempData["ErrorMessage"] = "Cập nhật thất bại";
            return View(request);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var result = await _systemUserBackendAPI.GetUserByID(id);
            if(result.IsSuccessed)
            {
                var user = result.Object;
                var data = new UserUpdateRequest()
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    PhoneNumber = user.PhoneNumber,
                    Address = user.Address,
                    DateOfBirth = user.DateOfBirth,
                    Email = user.Email,
                    ID = user.ID
                };
                return View(data);
            }    
            return RedirectToAction("Error", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            var result = await _systemUserBackendAPI.GetUserByID(id);
            return View(result.Object);
        }



        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _systemUserBackendAPI.GetUserByID(id);
            return PartialView("_DeteleModalPartial", result.Object);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(SystemUserModel model)
        {
            var result = await _systemUserBackendAPI.Delete(model.ID);
            if (result.IsSuccessed)
            {
                TempData["SuccessMessage"] = "Xóa thành công";
                return RedirectToAction("Index", "SystemUser");
            }
            
            TempData["ErrorMessage"] = result.Message;
            return RedirectToAction("Index", "SystemUser");
        }

        [HttpGet]
        public async Task<IActionResult> Login()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            if (!ModelState.IsValid)
                return View(ModelState);

            var result = await _systemUserBackendAPI.Authenticate(request);
            if(result.Message != "Success")
            {
                ModelState.AddModelError("", result.Message);
                return View();
            }
            var userPrincipal = this.ValidateToken(result.Object);
            var authProperties = new AuthenticationProperties
            {
                ExpiresUtc = DateTime.UtcNow.AddMinutes(10),
                IsPersistent = false
            };
            HttpContext.Session.SetString("Token", result.Object);
            await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        userPrincipal,
                        authProperties);
            return RedirectToAction("Index", "Home");

        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.Session.Remove("Token");
            return RedirectToAction("Login", "SystemUser");
        }


        private ClaimsPrincipal ValidateToken(string jwtToken)
        {
            IdentityModelEventSource.ShowPII = true;

            SecurityToken validatedToken;
            TokenValidationParameters validationParameters = new TokenValidationParameters();

            validationParameters.ValidateLifetime = true;
            validationParameters.ValidAudience = _config["Tokens:Issuer"];
            validationParameters.ValidIssuer = _config["Tokens:Issuer"];
            validationParameters.IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Tokens:Key"]));
            ClaimsPrincipal principal = new JwtSecurityTokenHandler().ValidateToken(jwtToken, validationParameters, out validatedToken);
            return principal;
        }
    }
}
