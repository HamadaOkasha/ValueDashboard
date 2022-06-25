
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
    public class RolePagesConfiguration : IEntityTypeConfiguration<RolePages>
    {
        public void Configure(EntityTypeBuilder<RolePages> entity)
        {
            entity.Property(e => e.RoleId).HasMaxLength(450);

            entity.HasOne(d => d.AdminPages)
                .WithMany(p => p.RolePages)
                .HasForeignKey(d => d.PageId)
                .HasConstraintName("FK_RolePages_AdminPages");

            entity.HasOne(d => d.AspNetRoles)
                .WithMany(p => p.RolePages)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("FK_RolePages_AspNetRoles");
        }
    }
}
