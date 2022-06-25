
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
    public class AspNetUserTokensConfiguration : IEntityTypeConfiguration<AspNetUserTokens>
    {
        public void Configure(EntityTypeBuilder<AspNetUserTokens> entity)
        {
            entity.Property(e => e.UserId).HasColumnType("nvarchar(450)");

            entity.HasKey(e => new { e.UserId, e.LoginProvider, e.Name });

            entity.Property(e => e.LoginProvider).HasMaxLength(128);

            entity.Property(e => e.Name).HasMaxLength(128);

            entity.HasOne(d => d.User)
                .WithMany(p => p.AspNetUserTokens)
                .HasForeignKey(d => d.UserId);
        }
    }
}
