using CoreBLOG.CORE.Map;
using CoreBLOG.MODEL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoreBLOG.MODEL.Maps
{
    public class UserMap : CoreMap<User>
    {
        public override void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");
            builder.Property(x => x.FirstName).HasMaxLength(100).IsRequired(true);
            builder.Property(x => x.LastName).HasMaxLength(100).IsRequired(true);
            builder.Property(x => x.Title).HasMaxLength(100).IsRequired(true);
            builder.Property(x => x.ImageURL).HasMaxLength(255).IsRequired(false);
            builder.Property(x => x.EmailAdress).HasMaxLength(255).IsRequired(true);
            builder.Property(x => x.Password).HasMaxLength(255).IsRequired(true);
            builder.Property(x => x.LastLogin).IsRequired(false);
            builder.Property(x => x.LastIpAdress).HasMaxLength(20).IsRequired(false);

            base.Configure(builder);
        }
    }
}
