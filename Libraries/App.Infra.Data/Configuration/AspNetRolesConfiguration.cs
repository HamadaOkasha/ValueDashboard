
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
    public class AspNetRolesConfiguration : IEntityTypeConfiguration<AspNetRoles>
    {
        public void Configure(EntityTypeBuilder<AspNetRoles> entity)
        {
            entity.Property(e => e.Id).HasColumnType("nvarchar(450)");


            entity.HasIndex(e => e.NormalizedName)
                .HasName("RoleNameIndex")
                .IsUnique()
                .HasFilter("([NormalizedName] IS NOT NULL)");

            entity.Property(e => e.Name).HasMaxLength(256);
            entity.Property(e => e.NameAr).HasMaxLength(256);

            entity.Property(e => e.NormalizedName).HasMaxLength(256);

            entity.HasData(new AspNetRoles()
            {
                Id="1",
                Name= "Administrator",
                NormalizedName = "Administrator",
                NameAr = "المدير العام",
            });

            entity.HasData(new AspNetRoles()
            {
                Id = "2",
                Name = "Customers",
                NormalizedName = "Customers",
                NameAr = "المستخدمين",
            });
        }
    }
}
