
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
    public class AspNetUsersConfiguration : IEntityTypeConfiguration<AspNetUsers>
    {
        public void Configure(EntityTypeBuilder<AspNetUsers> entity)
        {
            entity.Property(e => e.Id).HasColumnType("nvarchar(450)");

            entity.HasIndex(e => e.NormalizedEmail)
                    .HasName("EmailIndex");

            entity.HasIndex(e => e.NormalizedUserName)
                .HasName("UserNameIndex")
                .IsUnique()
                .HasFilter("([NormalizedUserName] IS NOT NULL)");


            entity.Property(e => e.FullName).HasMaxLength(500);

            entity.Property(e => e.Email).HasMaxLength(256);

            entity.Property(e => e.NormalizedEmail).HasMaxLength(256);

            entity.Property(e => e.NormalizedUserName).HasMaxLength(256);

            entity.Property(e => e.UserName).HasMaxLength(256);

            entity.HasData(new AspNetUsers()
            {
                Id = "3497bcc8-dabe-4a3f-a560-37f8fde72ace",
                UserName = "admin",
                NormalizedUserName = "ADMIN",
                Email = "ahmed_deve@hotmail.com",
                NormalizedEmail = "AHMED_DEVE@HOTMAIL.COM",
                EmailConfirmed = true,
                PasswordHash = "AQAAAAEAACcQAAAAEIzp5eNM/+PfyG5N2L1XhjQaGlTPB5CW0OETAEi+TmYDLSDuRS8Nstes+jt09As/Yg==",
                SecurityStamp = "U6GO4PPQ726KHDI2UGOFQJ72D3BX6QS4",
                ConcurrencyStamp = "05cb1028-e7c0-4c46-b833-e521d195dbe9",
                PhoneNumberConfirmed = true,
                TwoFactorEnabled = false,
                LockoutEnabled = false,
                AccessFailedCount = 0,
            });
        }
    }
}
