using Deposito.CatalogApi.Models;

namespace Deposito.CatalogApi.Repositories;

public interface ICategoryRepository
{
    Task<IEnumerable<Category>> GetAll();
    Task<Category?> GetById(int id);
    Task<Category?> GetByIdWithProducts(int id);
    Task<Category?> GetByName(string name);
    Task<Category> Create(Category category);
    Task<Category> Update(Category category);
    Task<Category> Delete(int id);
}
