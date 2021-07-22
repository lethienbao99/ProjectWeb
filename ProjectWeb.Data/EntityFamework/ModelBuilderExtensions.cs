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
            modelBuilder.Entity<AppConfig>().HasData(
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
                );
        }
    }
}
