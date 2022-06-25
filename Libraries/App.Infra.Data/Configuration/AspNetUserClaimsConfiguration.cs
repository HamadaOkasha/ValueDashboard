
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
    public class AspNetUserClaimsConfiguration : IEntityTypeConfiguration<AspNetUserClaims>
    {
        public void Configure(EntityTypeBuilder<AspNetUserClaims> entity)
        {
            entity.Property(e => e.UserId).HasColumnType("nvarchar(450)");
            entity.HasIndex(e => e.UserId);

            entity.Property(e => e.UserId).IsRequired();

            entity.HasOne(d => d.User)
                .WithMany(p => p.AspNetUserClaims)
                .HasForeignKey(d => d.UserId);
        }
    }
}
