
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
    public class AspNetUserLoginsConfiguration : IEntityTypeConfiguration<AspNetUserLogins>
    {
        public void Configure(EntityTypeBuilder<AspNetUserLogins> entity)
        {

            entity.Property(e => e.UserId).HasColumnType("nvarchar(450)");

            entity.HasKey(e => new { e.LoginProvider, e.ProviderKey });

            entity.HasIndex(e => e.UserId);

            entity.Property(e => e.LoginProvider).HasMaxLength(128);

            entity.Property(e => e.ProviderKey).HasMaxLength(128);

            entity.Property(e => e.UserId).IsRequired();

            entity.HasOne(d => d.User)
                .WithMany(p => p.AspNetUserLogins)
                .HasForeignKey(d => d.UserId);
        }
    }
}
