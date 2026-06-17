namespace Deposito.CatalogApi.Pagination;

public class ProductsFilter : ProductsParameters
{
    public string? Name { get; set; }
    public int? CategoryId { get; set; }
    public bool? LowStock { get; set; }
    public string? SortBy { get; set; }

}
