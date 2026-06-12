using Deposito.CatalogApi.Models;
using System.ComponentModel.DataAnnotations;

namespace Deposito.CatalogApi.DTOs;

public class CategoryDTO
{
    public int CategoryId { get; set; }
    [Required(ErrorMessage = "The Name field is required.")]
    [MinLength(2)]
    [MaxLength(100)]
    public string? Name { get; set; }
    public string? Description { get; set; }
    public ICollection<ProductDTO>? Products { get; set; }
}
