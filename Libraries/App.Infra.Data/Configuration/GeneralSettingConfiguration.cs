
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using App.Domain.Model;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Infra.Data.Configuration
{
    public class GeneralSettingConfiguration : IEntityTypeConfiguration<GeneralSetting>
    {
        public void Configure(EntityTypeBuilder<GeneralSetting> builder)
        {
            builder.Property(e => e.UserName).HasMaxLength(500);

            builder.Property(e => e.SMTP).HasMaxLength(500);
        }
    }

}
