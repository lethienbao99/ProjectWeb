﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProjectWeb.Data.Entities;
using ProjectWeb.Data.FluentAPIConfig;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectWeb.Data.EntityFamework
{
    public class ProjectWebDBContext : IdentityDbContext<SystemUser,AppRole,Guid>
    {
        public ProjectWebDBContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // READ DATABASE FROM MSSQL SERVER
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server =.;Database=ProjectWebDB;Trusted_Connection=True;");
            }

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Fluent API.
            modelBuilder.ApplyConfiguration(new AppConfigFluentAPI());
            modelBuilder.ApplyConfiguration(new ProductFluentAPI());
            modelBuilder.ApplyConfiguration(new CategoryFluentAPI());
            modelBuilder.ApplyConfiguration(new CartFluentAPI());
            modelBuilder.ApplyConfiguration(new OrderFluentAPI());
            modelBuilder.ApplyConfiguration(new OrderDetailFluentAPI());
            modelBuilder.ApplyConfiguration(new ProductCategoryFluentAPI());
            modelBuilder.ApplyConfiguration(new ImageFluentAPI());

            //Authen
            modelBuilder.ApplyConfiguration(new UserInformationFluentAPI());
            modelBuilder.ApplyConfiguration(new SystemUserFluentAPI());
            modelBuilder.ApplyConfiguration(new TokensFluentAPI());
            modelBuilder.ApplyConfiguration(new AppRoleFluentAPI());
            modelBuilder.ApplyConfiguration(new MessageFluentAPI());
            modelBuilder.Entity<IdentityUserClaim<Guid>>().ToTable("UserClaims");
            modelBuilder.Entity<IdentityUserRole<Guid>>().ToTable("UserRoles").HasKey(x => new { x.RoleId, x.UserId });
            modelBuilder.Entity<IdentityUserLogin<Guid>>().ToTable("UserLogins").HasKey(x => x.UserId);
            modelBuilder.Entity<IdentityUserToken<Guid>>().ToTable("UserTokens").HasKey(x => x.UserId);
            modelBuilder.Entity<IdentityRoleClaim<Guid>>().ToTable("RoleClaims");

            //Payment
            modelBuilder.ApplyConfiguration(new PaymentFluentAPI());
            modelBuilder.ApplyConfiguration(new PaymentSignatureFluentAPI());
            modelBuilder.ApplyConfiguration(new MerchantFluentAPI());

            //DataSeed.
            modelBuilder.Seed();
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<AppConfig> AppConfigs { get; set; }
        public DbSet<UserInformation> UserInformations { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Token> Tokens { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<PaymentSignature> PaymentSignatures { get; set; }
        public DbSet<Merchant> Merchants { get; set; }

    }
}
