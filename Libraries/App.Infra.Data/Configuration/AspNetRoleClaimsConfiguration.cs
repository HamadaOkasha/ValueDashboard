
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
    public class AspNetRoleClaimsConfiguration : IEntityTypeConfiguration<AspNetRoleClaims>
    {
        public void Configure(EntityTypeBuilder<AspNetRoleClaims> entity)
        {
            entity.Property(e => e.RoleId).HasColumnType("nvarchar(450)");

            entity.HasIndex(e => e.RoleId);

            entity.Property(e => e.RoleId).IsRequired();

            entity.HasOne(d => d.Role)
                .WithMany(p => p.AspNetRoleClaims)
                .HasForeignKey(d => d.RoleId);
        }
    }
}
