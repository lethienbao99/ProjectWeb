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
    public class ImageFluentAPI : IEntityTypeConfiguration<Image>
    {
        public void Configure(EntityTypeBuilder<Image> builder)
        {
            builder.ToTable("Images");
            builder.HasKey(x => x.ID);
            builder.Property(x => x.ImagePath).IsRequired();
            builder.Property(x => x.IsDefault).IsRequired(false);
            builder.Property(x => x.Caption).HasMaxLength(200).IsRequired(false);
            builder.HasOne(x => x.Product).WithMany(x => x.Images).HasForeignKey(x => x.ProductID);
            builder.Property(x => x.Sort).UseIdentityColumn();
            builder.Property(x => x.DateCreated).IsRequired(false);
            builder.Property(x => x.DateDeleted).IsRequired(false);
            builder.Property(x => x.DateUpdated).IsRequired(false);
            builder.Property(x => x.IsDelete).IsRequired(false);
        }
    }
}
