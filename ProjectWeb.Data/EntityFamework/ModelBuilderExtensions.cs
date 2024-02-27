using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ProjectWeb.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectWeb.Data.EntityFamework
{
    public static class ModelBuilderExtensions
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {



            var roleId = new Guid("EE976566-D4BE-407B-96D4-5C69DA8806A8");
            var adminId = new Guid("FD3BC079-8C61-4FF2-A5B7-278A58EC5273");

            var userID = new Guid("2AE5FECC-AEB6-4514-BFB5-34F2284ADBF8");

            modelBuilder.Entity<UserInformation>().HasData(new UserInformation
            {
                ID = userID,
                FirstName = "admin",
                LastName = "admin",
                PhoneNumber = "454545",
            });


            modelBuilder.Entity<AppRole>().HasData(new AppRole
            {
                Id = roleId,
                Name = "admin",
                NormalizedName = "admin",
                Description = "Administrator role"
            });

            var hasher = new PasswordHasher<SystemUser>();
            modelBuilder.Entity<SystemUser>().HasData(new SystemUser
            {
                Id = adminId,
                UserName = "admin",
                NormalizedUserName = "admin",
                Email = "lethienbao3012@gmail.com",
                NormalizedEmail = "lethienbao3012@gmail.com",
                EmailConfirmed = true,
                PasswordHash = hasher.HashPassword(null, "123abC!"),
                SecurityStamp = string.Empty,
                UserInfomationID = userID,
            });
            modelBuilder.Entity<IdentityUserRole<Guid>>().HasData(new IdentityUserRole<Guid>
            {
                RoleId = roleId,
                UserId = adminId
            });


            /*modelBuilder.Entity<AppConfig>().HasData(
                new AppConfig()
                {
                    ID = Guid.Parse("6E48C829-E01A-4204-B297-17F421915116"),
                    Key = "SuperAdmin",
                    Value = "True",
                    DateCreated = DateTime.Now
                }
                );
            modelBuilder.Entity<ProductCategory>().HasData(
                new ProductCategory()
                {
                    ID = Guid.Parse("628DA97C-6DDB-4AB4-9BB8-55B429B50DC4"),
                    ProductID = Guid.Parse("E75990A1-04DD-4413-9644-1BC157C9E477"),
                    CategoryID = Guid.Parse("3BC23769-D612-40C9-8D7A-6B15C621302D"),
                    DateCreated = DateTime.Now
                }
                );*/

            modelBuilder.Entity<Merchant>().HasData(new Merchant
            {
                ID = Guid.NewGuid(),
                MerchantName = "VNPAY",
                ShortName = "VNPay",
                Version = "2.1.0",
                Tmncode = "APPZFC7N",
                MerchantPayLink = "https://sandbox.vnpayment.vn/paymentv2/vpcpay.html",
                SerectKey = "YONPSVXYSUNSPVKIUOOOWXASIHLLYIFS",
                MerchantReturnUrl = "https://localhost:5001",
                MerchantIpnUrl = "https://localhost:5001",
                IsActive = true,
            });
        }
    }
}
