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
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
          
            builder.Property(e => e.Name).HasMaxLength(200);
          
        }
    }
}
