using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ProjectWeb.Common.Exceptions;
using ProjectWeb.Common.IServices;
using ProjectWeb.Common.Repositories;
using ProjectWeb.Data.Entities;
using ProjectWeb.Data.EntityFamework;
using ProjectWeb.Models.CommonModels;
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

        public async Task<ResultMessage<string>> Authenticate(LoginRequest request)
        {
            var user = await _userManager.FindByNameAsync(request.Username);
            if (user == null) return new ResultObjectError<string>("User không tồn tại."); 

            var result = await _signInManager.PasswordSignInAsync(user, request.Password, request.IsRememberMe, true);
            if (!result.Succeeded)
                return new ResultObjectError<string>("Sai mật khẩu.");

            var userInfo = _context.UserInformations.FirstOrDefault(x => x.ID == user.UserInfomationID);

            var roles = await _userManager.GetRolesAsync(user);
            var claims = new[]
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.GivenName, userInfo.FirstName),
                new Claim(ClaimTypes.Role, string.Join(";", roles)),
                new Claim(ClaimTypes.Name, request.Username),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Tokens:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(_config["Tokens:Issuer"],
                _config["Tokens:Issuer"],
                claims,
                expires: DateTime.Now.AddHours(3),
                signingCredentials: creds);

            var JwtToken =  new JwtSecurityTokenHandler().WriteToken(token);
            return new ResultObjectSuccess<string>(new JwtSecurityTokenHandler().WriteToken(token));

        }

        public async Task<ResultMessage<bool>> Register(SignUpRequest request)
        {
            var user = await _userManager.FindByNameAsync(request.Username);
            if(user != null)
                return new ResultObjectError<bool>("Tài khoản đã tồn tại");

            if (await _userManager.FindByEmailAsync(request.Email) != null)
                return new ResultObjectError<bool>("Email đã tồn tại"); ;
            var userInfo = new UserInformation()
            {
                ID = Guid.NewGuid(),
                FirstName = request.FirstName,
                LastName = request.LastName,
                DateOfBirth = request.DateOfBirth,
                DateCreated = DateTime.Now,
                Status = request.Status,
                PhoneNumber = request.PhoneNumber,
                Address = request.Address
            };
            _context.UserInformations.Add(userInfo);
            
            var systemUser = new SystemUser()
            {
                UserName = request.Username,
                DateCreated = DateTime.Now,
                UserInfomationID = userInfo.ID,
                Email = request.Email
            };

            var result = await _userManager.CreateAsync(systemUser, request.Password);
            if (result.Succeeded)
            {
                _context.SaveChanges();
                return new ResultObjectSuccess<bool>();
            }

            return new ResultObjectError<bool>("Đăng ký thất bại."); ;


        }


        public async Task<ResultMessage<PageResultModel<SystemUserModel>>> GetUserPaging(UserPagingRequest request)
        {
            var user = from su in _context.Users
                       join ui in _context.UserInformations on su.UserInfomationID equals ui.ID
                       where ui.IsDelete == null && su.IsDelete == null
                       select new { su, ui };
                      
            if (!string.IsNullOrEmpty(request.Keyword))
            {
                user = user.Where(x => x.su.UserName.Contains(request.Keyword)
                || x.ui.FirstName.Contains(request.Keyword) || x.ui.LastName.Contains(request.Keyword));
            }

            int totalRow = user.Count();

            var data = await user.Skip((request.PageIndex - 1) * request.PageSize).Take(request.PageSize).OrderBy(x => x.ui.Sort)
                 .Select(x => new SystemUserModel()
                 {
                     ID = x.su.Id,
                     Username = x.su.UserName,
                     FirstName = x.ui.FirstName,
                     LastName = x.ui.LastName,
                     PhoneNumber = x.ui.PhoneNumber,
                     Address = x.ui.Address,
                     Status = x.ui.Status,
                     DateOfBirth = x.ui.DateOfBirth,
                     Email = x.su.Email,
                     DateCreated = x.ui.DateCreated
                 }).Distinct().ToListAsync();

            var pagedResult = new PageResultModel<SystemUserModel>()
            {
                TotalRecords = totalRow,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
                Items = data
            };

            return new ResultObjectSuccess<PageResultModel<SystemUserModel>>(pagedResult);
        }



        public async Task<ResultMessage<bool>> Update(Guid ID, UserUpdateRequest request)
        {
            if (await _userManager.Users.AnyAsync(x => x.Email == request.Email && x.Id != ID))
            {
                return new ResultObjectError<bool>("Emai đã tồn tại");
            }
            var user = await _userManager.FindByIdAsync(ID.ToString());
            user.Email = request.Email;

            var userInfo = await _context.UserInformations.FirstOrDefaultAsync(x => x.ID == user.UserInfomationID && x.IsDelete == null);
            userInfo.FirstName = request.FirstName;
            userInfo.LastName = request.LastName;
            userInfo.PhoneNumber = request.PhoneNumber;
            userInfo.Address = request.Address;
            userInfo.DateOfBirth = request.DateOfBirth;
            userInfo.DateUpdated = DateTime.Now;

            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                _context.SaveChanges();
                return new ResultObjectSuccess<bool>();
            }
            return new ResultObjectError<bool>("Cập nhật không thành công");
        }

        public async Task<ResultMessage<SystemUserModel>> GetUserByID(Guid ID)
        {
            var user = await _userManager.FindByIdAsync(ID.ToString());
            if(user == null)
                return new ResultObjectError<SystemUserModel>("User không tồn tại.");


            var roles = await _userManager.GetRolesAsync(user);
            var userInfo = await _context.UserInformations.FirstOrDefaultAsync(s => s.ID == user.UserInfomationID && s.IsDelete == null);

            var data = new SystemUserModel()
            {
                ID = user.Id,
                Username = user.UserName,
                Email = user.Email,
                FirstName = userInfo.FirstName,
                LastName = userInfo.LastName,
                PhoneNumber = userInfo.PhoneNumber,
                Address = userInfo.Address,
                DateOfBirth = userInfo.DateOfBirth,
                DateCreated = userInfo.DateCreated,
                DateUpdated = userInfo.DateUpdated,
                Status = userInfo.Status,
                Roles = roles
            };
            return new ResultObjectSuccess<SystemUserModel>(data);
        }

        public async Task<ResultMessage<bool>> Delete(Guid ID)
        {
            var user = await _userManager.FindByIdAsync(ID.ToString());

            if (user == null)
                return new ResultObjectError<bool>("User không tồn tại.");
            else if(user.UserName == "admin")
                return new ResultObjectError<bool>("Không được xóa superadmin");

            var userInfo = await _context.UserInformations.FirstOrDefaultAsync(s => s.ID == user.UserInfomationID && s.IsDelete == null);
            if (userInfo != null)
                _context.UserInformations.Remove(userInfo);

            var result = await _userManager.DeleteAsync(user);
            if(result.Succeeded)
            {
                _context.SaveChanges();
                return new ResultObjectSuccess<bool>();
            }

            return new ResultObjectError<bool>("Fail");

        }

        public async Task<ResultMessage<bool>> RoleAssign(Guid ID, RoleAssignRequest request)
        {
            var user = await _userManager.FindByIdAsync(request.ID.ToString());
            if (user == null)
            {
                return new ResultObjectError<bool>("User is not exists");
            }
            var userRoles = await _userManager.GetRolesAsync(user);

            if (request.Roles != null)
            {
                foreach (var role in request.Roles)
                {
                    //Nếu có check và chưa tồn tại role này thì add vào.
                    if (role.Selected && !userRoles.Contains(role.Name))
                    {
                        await _userManager.AddToRoleAsync(user, role.Name);

                    }
                    
                    //Bỏ check thì move role.
                    else if (!role.Selected || userRoles.Contains(role.Name))
                    {
                        await _userManager.RemoveFromRoleAsync(user, role.Name);
                    }

                }
               
            }

            return new ResultObjectSuccess<bool>();
        }
    }
}
