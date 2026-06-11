using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Deposito.CatalogApi.DTOs;

public class ProductDTO
{
    public int ProductId { get; set; }
    [Required(ErrorMessage = "The Name field is required.")]
    [MinLength(2)]
    [MaxLength(150)]
    public string? Name { get; set; }

    [MinLength(5)]
    [MaxLength(200)]
    public string? Description { get; set; }
    [Required(ErrorMessage = "The Price is Required")]
    [Column(TypeName = "decimal(12,2)")]
    public decimal SalePrice { get; set; }
    [Required(ErrorMessage = "The Cost is Required")]
    [Column(TypeName = "decimal(12,2)")]
    public decimal CostPrice { get; set; }
    [Required(ErrorMessage = "The Stock is Required")]
    [Range(1, 9999)]
    public decimal StockQuantity { get; set; }
    [Required(ErrorMessage = "The Minimum Stock is Required")]
    [Range(1, 9999)]
    public decimal MinimumStock { get; set; }
    [MaxLength(250)]
    [DisplayName("Product Image")]
    public string? ImageUrl { get; set; }

    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }


    [Required(ErrorMessage = "The CategoryId field is required.")]
    public int CategoryId { get; set; }
    public string? CategoryName { get; set; }
    [JsonIgnore]
    public CategoryDTO? Category { get; set; }

    public decimal Profit => SalePrice - CostPrice;
    public bool IsLowStock => StockQuantity < MinimumStock;
}
