using Deposito.CatalogApi.DTOs;
using Deposito.CatalogApi.Models;
using Deposito.CatalogApi.Pagination;

namespace Deposito.CatalogApi.Services;

public interface IProductService
{
    Task<IEnumerable<ProductDTO>> GetProducts();
    Task<IEnumerable<ProductDTO?>> GetProductsByCategory(int id);
    Task<ProductDTO> GetProductById(int id);
    Task<IEnumerable<ProductDTO>> GetProductByName(string name);
    Task<PagedList<ProductDTO>> GetProducts(ProductsParameters productsParams);

    Task<ProductDTO> AddProduct(ProductDTO productDto);
    Task UpdateProduct(ProductDTO productDto);
    Task RemoveProduct(int id);
}
