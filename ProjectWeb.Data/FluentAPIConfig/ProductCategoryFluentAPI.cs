using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjectWeb.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectWeb.Data.FluentAPIConfig
{
    public class ProductCategoryFluentAPI : IEntityTypeConfiguration<ProductCategory>
    {
        public void Configure(EntityTypeBuilder<ProductCategory> builder)
        {
            builder.ToTable("ProductCategories");
            builder.HasKey(x => new { x.ID, x.ProductID, x.CategoryID });
            builder.HasOne(x => x.Product).WithMany(x => x.ProductCategories).HasForeignKey(x => x.ProductID);
            builder.HasOne(x => x.Category).WithMany(x => x.ProductCategories).HasForeignKey(x => x.CategoryID);
            builder.Property(x => x.DateCreated).IsRequired(false);
            builder.Property(x => x.DateDeleted).IsRequired(false);
            builder.Property(x => x.DateUpdated).IsRequired(false);
            builder.Property(x => x.IsDelete).IsRequired(false);
        }
    }
}
