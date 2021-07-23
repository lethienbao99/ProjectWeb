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
    public class AppRoleFluentAPI : IEntityTypeConfiguration<AppRole>
    {
        public void Configure(EntityTypeBuilder<AppRole> builder)
        {
            builder.ToTable("AppRoles");
            builder.Property(x => x.Sort).UseIdentityColumn();
            builder.Property(x => x.DateCreated).IsRequired(false);
            builder.Property(x => x.DateDeleted).IsRequired(false);
            builder.Property(x => x.DateUpdated).IsRequired(false);
            builder.Property(x => x.IsDelete).IsRequired(false);
        }
    }
}
