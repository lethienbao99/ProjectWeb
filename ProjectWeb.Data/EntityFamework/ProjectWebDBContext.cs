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
    public class ProjectWebDBContext : DbContext
    {
        public ProjectWebDBContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new AppConfigFluentAPI());
            modelBuilder.ApplyConfiguration(new ProductFluentAPI());
            modelBuilder.ApplyConfiguration(new CategoryFluentAPI());
            modelBuilder.ApplyConfiguration(new CartFluentAPI());
            modelBuilder.ApplyConfiguration(new OrderFluentAPI());
            modelBuilder.ApplyConfiguration(new OrderDetailFluentAPI());
            modelBuilder.ApplyConfiguration(new ProductCategoryFluentAPI());

            //base.OnModelCreating(modelBuilder);
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<AppConfig> AppConfigs { get; set; }

    }
}
