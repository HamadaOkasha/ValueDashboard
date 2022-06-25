
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
    public class AdminPagesConfiguration : IEntityTypeConfiguration<AdminPages>
    {
        public void Configure(EntityTypeBuilder<AdminPages> builder)
        {
            builder.Property(e => e.Id).ValueGeneratedNever();

            builder.Property(e => e.Icon).HasMaxLength(200);

            builder.Property(e => e.PageTitle).HasMaxLength(200);

            builder.Property(e => e.PageUrl).HasMaxLength(200);

        }
    }
}