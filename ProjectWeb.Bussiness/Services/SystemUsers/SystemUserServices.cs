using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ProjectWeb.Common.Exceptions;
using ProjectWeb.Common.IServices;
using ProjectWeb.Common.Repositories;
using ProjectWeb.Data.Entities;
using ProjectWeb.Data.EntityFamework;
using ProjectWeb.Models.SystemUsers;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ProjectWeb.Bussiness.Services.SystemUsers
{
    public class SystemUserServices : ISystemUserServices
    {
        private readonly ProjectWebDBContext _context;
        private readonly UserManager<SystemUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly IConfiguration _config;

        private readonly SignInManager<SystemUser> _signInManager;
        public SystemUserServices(ProjectWebDBContext context, 
            UserManager<SystemUser> userManager, 
            SignInManager<SystemUser> signInManager,
            RoleManager<AppRole> roleManager,
            IConfiguration config) 
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _config = config;
        }

        public async Task<string> Authenticate(LoginRequest request)
        {
            var user = await _userManager.FindByNameAsync(request.Username);
            if (user == null) return null;

            var result = await _signInManager.PasswordSignInAsync(user, request.Password, request.IsRememberMe, true);
            if (!result.Succeeded)
                return null;

            var userInfo = _context.UserInformations.FirstOrDefault(x => x.ID == user.UserInfomationID);

            var roles = await _userManager.GetRolesAsync(user);
            var claims = new[]
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.GivenName, userInfo.FirstName),
                new Claim(ClaimTypes.Role, string.Join(";", roles)),
                new Claim(ClaimTypes.Name, request.Username),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Tokens:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(_config["Tokens:Issuer"],
                _config["Tokens:Issuer"],
                claims,
                expires: DateTime.Now.AddHours(3),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);


        }

        public async Task<bool> Register(SignUpRequest request)
        {
            var userInfo = new UserInformation()
            {
                ID = Guid.NewGuid(),
                FirstName = request.FirstName,
                LastName = request.LastName,
                DateOfBirth = request.DateOfBirth,
                DateCreated = DateTime.Now,
                Status = request.Status
            };
            _context.UserInformations.Add(userInfo);
            _context.SaveChanges();
            var systemUser = new SystemUser()
            {
                UserName = request.Username,
                DateCreated = DateTime.Now,
                UserInfomationID = userInfo.ID,
                Email = request.Email
            };

            var result = await _userManager.CreateAsync(systemUser, request.Password);
            if (result.Succeeded)
                return true;

            return false;

        }
    }
}
