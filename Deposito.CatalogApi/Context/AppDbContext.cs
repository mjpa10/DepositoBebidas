using Deposito.CatalogApi.Models;
using Microsoft.EntityFrameworkCore;

namespace Deposito.CatalogApi.Context;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions options) : base(options){}
    public DbSet<Product> Products { get; set; }
    public DbSet<Category> Categories { get; set; }
    protected override void OnModelCreating(ModelBuilder mb)
    {
        // ── Category ──────────────────────────────────────────────
        mb.Entity<Category>(e =>
        {
            e.ToTable("Categories");

            e.HasKey(c => c.CategoryId);

            e.Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(100);

            e.Property(c => c.Description)
                .HasMaxLength(500);

            // nome da categoria deve ser único
            e.HasIndex(c => c.Name)
                .IsUnique();
        });

        // ── Product ───────────────────────────────────────────────
        mb.Entity<Product>(e =>
        {
            e.ToTable("Products");

            e.HasKey(p => p.ProductId);

            e.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(150);

            e.Property(p => p.Description)
                .HasMaxLength(500);

            e.Property(p => p.SalePrice)
                .HasColumnType("decimal(10,2)");

            e.Property(p => p.CostPrice)
                .HasColumnType("decimal(10,2)");

            e.Property(p => p.StockQuantity)
                .HasColumnType("decimal(10,2)")
                .HasDefaultValue(0);

            e.Property(p => p.MinimumStock)
                .HasColumnType("decimal(10,2)")
                .HasDefaultValue(5);

            e.Property(p => p.ImageUrl)
                .HasMaxLength(500);

            e.Property(p => p.IsActive)
                .HasDefaultValue(true);

            // propriedades calculadas — ignoradas pelo EF
            e.Ignore(p => p.Profit);
            e.Ignore(p => p.IsLowStock);

            // ── Relacionamento Category → Products (1:N) ──────────
            e.HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Restrict); // não deixa deletar categoria com produtos

            // ── Índices ───────────────────────────────────────────
            e.HasIndex(p => p.CategoryId);   // agiliza busca por categoria
            e.HasIndex(p => p.IsActive);     // agiliza filtro de produtos ativos
        });
    }
}
