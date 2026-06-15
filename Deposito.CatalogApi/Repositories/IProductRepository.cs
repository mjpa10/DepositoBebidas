using Deposito.CatalogApi.Models;
using Deposito.CatalogApi.Pagination;

namespace Deposito.CatalogApi.Repositories;

public interface IProductRepository
{
    Task<IEnumerable<Product>> GetAll();
    Task<IEnumerable<Product>> GetByCategory(int id);
    Task<IEnumerable<Product>> GetByName(string name);
    Task<IEnumerable<Product>> GetProducts(ProductsParameters productsParameters);

    Task<Product> GetById(int id);
 
    Task<Product> Create(Product product);
    Task<Product> Update(Product product);
    Task<Product> Delete(int id);
}
