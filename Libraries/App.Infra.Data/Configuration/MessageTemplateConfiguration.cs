
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
    public class MessageTemplateConfiguration : IEntityTypeConfiguration<MessageTemplate>
    {
        public void Configure(EntityTypeBuilder<MessageTemplate> builder)
        {
            builder.Property(e => e.Title).HasMaxLength(400);

            builder.HasData(
            new MessageTemplate() { Id = 1, Title = "استعادة كلمة المرور", IsActive = true ,Description = "{Name} {Url}" , TypeId = 1},
            new MessageTemplate() { Id = 2, Title = "استعادة كلمة المرور", IsActive = true ,Description = "{Name} {Url}" , TypeId = 2 },
            new MessageTemplate() { Id = 4, Title = "تفعيل حساب", IsActive = true ,Description = "{Name} {Url}" , TypeId = 2 },
            new MessageTemplate() { Id = 5, Title = "ايقاف حساب", IsActive = true ,Description = "{Name} {Url}" , TypeId = 1 },
            new MessageTemplate() { Id = 6, Title = "ايقاف حساب", IsActive = true ,Description = "{Name} {Url}" , TypeId = 2 }
            );
        }
    }
}
