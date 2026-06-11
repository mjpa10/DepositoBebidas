using AutoMapper;
using Deposito.CatalogApi.DTOs;
using Deposito.CatalogApi.Models;
using Deposito.CatalogApi.Repositories;

namespace Deposito.CatalogApi.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;


        public CategoryService(ICategoryRepository categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CategoryDTO>> GetCategories()
        {
            var categories = await _categoryRepository.GetAll();
            return _mapper.Map<IEnumerable<CategoryDTO>>(categories);
        }

        public async Task<CategoryDTO> GetCategoryById(int id)
        {
            var category = await _categoryRepository.GetById(id);
            return _mapper.Map<CategoryDTO>(category);
        }

        public async Task<CategoryDTO> GetCategoryProducts(int id)
        {
            var category = await _categoryRepository.GetByIdWithProducts(id);
            return _mapper.Map<CategoryDTO>(category);
        }

        public async Task<CategoryDTO> AddCategory(CategoryDTO categoryDto)
        {
            var category = _mapper.Map<Category>(categoryDto);
            await _categoryRepository.Create(category);
            categoryDto.CategoryId = category.CategoryId;
            return _mapper.Map<CategoryDTO>(category);

        }

        public async Task UpdateCategory(CategoryDTO categoryDto)
        {
            var category = _mapper.Map<Category>(categoryDto);
            await _categoryRepository.Update(category);
        }

        public async Task RemoveCategory(int id)
        {
            await _categoryRepository.Delete(id);
        }
    }
}
