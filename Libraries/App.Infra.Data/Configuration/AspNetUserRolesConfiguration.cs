
using App.Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Infra.Data.Configuration
{
    public class AspNetUserRolesConfiguration : IEntityTypeConfiguration<AspNetUserRoles>
    {
        public void Configure(EntityTypeBuilder<AspNetUserRoles> entity)
        {
            entity.Property(e => e.UserId).HasColumnType("nvarchar(450)");
            entity.Property(e => e.RoleId).HasColumnType("nvarchar(450)");

            entity.HasKey(e => new { e.UserId, e.RoleId });

            entity.HasIndex(e => e.RoleId);

            entity.HasOne(d => d.Role)
                .WithMany(p => p.AspNetUserRoles)
                .HasForeignKey(d => d.RoleId);

            entity.HasOne(d => d.User)
                .WithMany(p => p.AspNetUserRoles)
                .HasForeignKey(d => d.UserId);

            entity.HasData(new AspNetUserRoles()
            {
                UserId = "3497bcc8-dabe-4a3f-a560-37f8fde72ace",
                RoleId = "1"
            });
        }
    }
}
