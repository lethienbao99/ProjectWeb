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
    public class CartFluentAPI : IEntityTypeConfiguration<Cart>
    {
        public void Configure(EntityTypeBuilder<Cart> builder)
        {
            builder.ToTable("Carts");
            builder.HasKey(x => x.ID);
            builder.Property(x => x.Sort).UseIdentityColumn();
            builder.HasOne(x => x.Product).WithMany(x => x.Carts).HasForeignKey(x => x.ProductID);


            builder.HasOne(x => x.SystemUser).WithMany(x => x.Carts).HasForeignKey(x => x.UserID);
            builder.Property(x => x.DateCreated).IsRequired(false);
            builder.Property(x => x.DateDeleted).IsRequired(false);
            builder.Property(x => x.DateUpdated).IsRequired(false);
            builder.Property(x => x.IsDelete).IsRequired(false);
         
        }
    }
}
