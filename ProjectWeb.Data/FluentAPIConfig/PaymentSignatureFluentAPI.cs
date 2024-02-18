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
    public class PaymentSignatureFluentAPI : IEntityTypeConfiguration<PaymentSignature>
    {
        public void Configure(EntityTypeBuilder<PaymentSignature> builder)
        {
            builder.ToTable("PaymentSignatures");
            builder.HasKey(x => x.ID);
            builder.HasOne(x => x.Payment).WithMany(x => x.PaymentSignatures).HasForeignKey(x => x.PaymentID);
        }
    }
}
