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
    public class MessageFluentAPI : IEntityTypeConfiguration<Message>
    {
        public void Configure(EntityTypeBuilder<Message> builder)
        {
            builder.ToTable("Messages");
            builder.HasKey(x => x.ID);
            builder.Property(x => x.MessageText).HasMaxLength(200).IsRequired();
            builder.Property(x => x.Sort).UseIdentityColumn();
            builder.Property(x => x.DateCreated).IsRequired(false);
            builder.Property(x => x.DateDeleted).IsRequired(false);
            builder.Property(x => x.DateUpdated).IsRequired(false);
            builder.Property(x => x.IsDelete).IsRequired(false);
            builder.Property(x => x.Guest).HasMaxLength(50).IsRequired(false);
            builder.Property(x => x.TitleText).HasMaxLength(100).IsRequired(false);

            builder.HasOne(x => x.Product).WithMany(x => x.Messages).HasForeignKey(x => x.ProductID);

        }
    }
}
