namespace Deposito.CatalogApi.Models;

public class Product
{
    public int ProductId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal SalePrice { get; set; } //preco de venda ao cliente
    public decimal CostPrice { get; set; } //preco de custo para o deposito, usado para calcular lucro
    public decimal StockQuantity { get; set; } = 0;
    public decimal MinimumStock { get; set; } = 5;
    public string? ImageUrl { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    public int CategoryId { get; set; }
    public Category? Category { get; set; }

    // calculadas — configuradas com .Ignore() no DbContext
    public decimal Profit => SalePrice - CostPrice;
    public bool IsLowStock => StockQuantity < MinimumStock;
}
