using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace tm_api.Models
{
    public class ProductContext : DbContext
    {
        public ProductContext(DbContextOptions<ProductContext> options) : base(options)
        {
        }

        public DbSet<ProductDto> ProductDto { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<ProductDto>(entity => {
                entity.HasIndex(e => e.Code).IsUnique();
                builder.Entity<ProductDto>()
                .HasAlternateKey(c => c.Code)
                .HasName("AlternateKey_Code");
            });
        }
    }
}
