using Microsoft.EntityFrameworkCore;
using MiniShop.Domain.Entities;

namespace MiniShop.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options): base(options)
    {
    }

    public DbSet<Product> Products => Set<Product>();

    protected override void OnModelCreating(
        ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Product>()
            .HasDiscriminator<string>("ProductType")
            .HasValue<PhysicalProduct>("Physical")
            .HasValue<DigitalProduct>("Digital");

        modelBuilder.Entity<Product>()
            .Property(product => product.Id)
            .ValueGeneratedOnAdd();

        modelBuilder.Entity<Product>()
            .Property(product => product.Name)
            .HasMaxLength(200)
            .IsRequired();

        modelBuilder.Entity<Product>()
            .Property(product => product.Price)
            .HasPrecision(18, 2);
    }
}