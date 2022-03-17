using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.Configuration
{
    public class ProductoConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.Property(p => p.Code)
                .IsRequired().HasMaxLength(50);
            builder.Property(p => p.Sku)
                .IsRequired().HasMaxLength(50);
            builder.Property(p => p.Currency)
                .IsRequired().HasMaxLength(10);
            builder.Property(p => p.Stock)
                .HasColumnType("decimal(18,2)");
            builder.Property(p => p.Price)
                .HasColumnType("decimal(18,2)");
            builder.Property(p => p.Iva)
                .HasColumnType("decimal(18,2)");
            builder.Property(p => p.Ii)
                .HasColumnType("decimal(18,2)");
        }
    }
}
