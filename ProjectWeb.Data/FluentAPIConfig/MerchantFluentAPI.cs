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
    public class MerchantFluentAPI : IEntityTypeConfiguration<Merchant>
    {
        public void Configure(EntityTypeBuilder<Merchant> builder)
        {
            builder.ToTable("Merchants");
            builder.Property(s => s.MerchantName).IsRequired().HasMaxLength(50);
            builder.Property(s => s.MerchantPayLink).IsRequired().HasMaxLength(1000);
            builder.Property(s => s.MerchantIpnUrl).IsRequired().HasMaxLength(1000);
            builder.Property(s => s.MerchantReturnUrl).IsRequired().HasMaxLength(1000);
            builder.Property(s => s.SerectKey).IsRequired().HasMaxLength(500);
            builder.HasKey(x => x.ID);
        }
    }
}
