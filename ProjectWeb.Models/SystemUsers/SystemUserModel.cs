﻿using ProjectWeb.Models.CommonModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectWeb.Models.SystemUsers
{
    public class SystemUserModel 
    {
        public Guid ID { get; set; }
        public string Address { get; set; }
        public string Status { get; set; }
        public string Email { get; set; }
        public int Sort { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? DateUpdated { get; set; }
        public DateTime? DateDeleted { get; set; }
        public bool? IsDelete { get; set; }

        public Guid UserInfomationID { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        [DataType(DataType.Date)]
        public DateTime? DateOfBirth { get; set; }
        public string Username { get; set; }
        public IList<string> Roles { get; set; }
    }

    public class LoginRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public bool IsRememberMe { get; set; }
    }

    public class SignUpRequest
    {
        [Display(Name = "Tài khoản")]
        public string Username { get; set; }
        [Display(Name = "Mật khẩu")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Display(Name = "Nhập lại mật khẩu")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
        [Display(Name = "Họ")]
        public string FirstName { get; set; }
        [Display(Name = "Tên")]
        public string LastName { get; set; }
        [Display(Name = "SDT")]
        public string PhoneNumber { get; set; }
        [Display(Name = "Địa chỉ")]
        public string Address { get; set; }
        [Display(Name = "Trạng thái")]
        public string Status { get; set; }
        public string Email { get; set; }
        [Display(Name = "Ngày sinh")]
        [DataType(DataType.Date)]
        public DateTime? DateOfBirth { get; set; }
    }

    public class UserPagingRequest : PagingRequestBase
    {
        public string Keyword { get; set; }
    }
}
