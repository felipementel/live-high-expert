using Microsoft.EntityFrameworkCore;
using ProductApi.Domain;

namespace ProductApi.Infrastructure.Persistence;

public sealed class ProductDbContext(DbContextOptions<ProductDbContext> options) : DbContext(options)
{
    public DbSet<Product> Products => Set<Product>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(product => product.Id);
            entity.Property(product => product.Name).IsRequired().HasMaxLength(200);
            entity.Property(product => product.Description).HasMaxLength(1000);
            entity.Property(product => product.Price).HasPrecision(12, 2);
        });
    }
}
