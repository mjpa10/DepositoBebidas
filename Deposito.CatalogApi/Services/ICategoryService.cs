using Deposito.CatalogApi.DTOs;

namespace Deposito.CatalogApi.Services;

public interface ICategoryService
{
    Task<IEnumerable<CategoryDTO>> GetCategories();
    Task<CategoryDTO> GetCategoryById(int id);
    Task<CategoryDTO> GetCategoryProducts(int id);
    Task<CategoryDTO> AddCategory(CategoryDTO categoryDto);
    Task UpdateCategory(CategoryDTO categoryDto);
    Task RemoveCategory(int id);
}
