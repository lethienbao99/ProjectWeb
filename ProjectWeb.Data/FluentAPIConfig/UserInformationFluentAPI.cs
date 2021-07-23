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
    public class UserInformationFluentAPI : IEntityTypeConfiguration<UserInformation>
    {
        public void Configure(EntityTypeBuilder<UserInformation> builder)
        {
            builder.ToTable("UserInformations");
            builder.HasKey(x => x.ID);
            builder.HasOne(x => x.SysUser).WithOne(x => x.UserInfomation).HasForeignKey<SystemUser>(x => x.UserInfomationID);
            builder.Property(x => x.FirstName).IsRequired().HasMaxLength(100);
            builder.Property(x => x.LastName).IsRequired().HasMaxLength(100);
            builder.Property(x => x.Sort).UseIdentityColumn();
            builder.Property(x => x.DateOfBirth).IsRequired(false);
            builder.Property(x => x.DateCreated).IsRequired(false);
            builder.Property(x => x.DateDeleted).IsRequired(false);
            builder.Property(x => x.DateUpdated).IsRequired(false);
            builder.Property(x => x.IsDelete).IsRequired(false);
        }
    }
}
