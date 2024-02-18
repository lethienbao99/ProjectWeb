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
    public class PaymentFluentAPI : IEntityTypeConfiguration<Payment>
    {
        public void Configure(EntityTypeBuilder<Payment> builder)
        {
            builder.ToTable("Payments");
            builder.HasKey(x => x.ID);
            builder.Property(x => x.Currency).IsRequired().HasMaxLength(10);
            builder.Property(x => x.Language).IsRequired().HasMaxLength(5);
            builder.Property(x => x.Content).IsRequired().HasMaxLength(100);
            builder.Property(x => x.RequiredAmount).IsRequired();
            builder.HasOne(x => x.Merchant).WithMany(x => x.Payments).HasForeignKey(x => x.MerchantID);
            builder.HasOne(x => x.Order).WithOne(x => x.Payment).HasForeignKey<Payment>(x => x.OrderID);

        }
    }
}
