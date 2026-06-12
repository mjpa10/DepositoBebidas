using Deposito.CatalogApi.DTOs;
using Deposito.CatalogApi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Deposito.CatalogApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<IActionResult> GetCategories()
        {
            var categoriesDto = await _categoryService.GetCategories();
            if (categoriesDto == null)
            {
                return NotFound("Categories not found");
            }
            return Ok(categoriesDto);
        }

        [HttpGet("{id:int}", Name = "GetCategory")]
        public async Task<IActionResult> GetCategoryById(int id)
        {
            var categoryDto = await _categoryService.GetCategoryById(id);
            if (categoryDto == null)
            {
                return NotFound($"Category com ID {id} not found");
            }
            return Ok(categoryDto);
        }

        // Endpoint para buscar a categoria trazendo também os seus produtos relacionados
        [HttpGet("{id:int}/products")]
        public async Task<ActionResult<CategoryDTO>> GetCategoryProducts(int id)
        {
            var categoryWithProducts = await _categoryService.GetCategoryProducts(id);
            if (categoryWithProducts == null)
            {
                return NotFound($"Category com ID {id} não encontrada.");
            }
            return Ok(categoryWithProducts);
        }

        [HttpPost]
        public async Task<ActionResult<CategoryDTO>> Post([FromBody] CategoryDTO categoryDto)
        {
            if (categoryDto == null) return BadRequest("Dados inválidos.");

            var newCategory = await _categoryService.AddCategory(categoryDto);

            if (newCategory is null) return BadRequest($"Category com nome {categoryDto.Name} já existe.");

            // Retorna o status 201 Created e preenche o Header Location com a URL do GetById
            return new CreatedAtRouteResult("GetCategory", new { id = categoryDto.CategoryId },
            categoryDto);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int id, [FromBody] CategoryDTO categoryDto)
        {
            if (id != categoryDto.CategoryId)
                return BadRequest("O ID do caminho não corresponde ao ID da categoria enviada.");

            if (categoryDto == null)
                return BadRequest("Dados inválidos.");

            var category = await _categoryService.GetCategoryById(id);

            if (category is null)
                return NotFound($"Category com id {id} não encontrada.");

            await _categoryService.UpdateCategory(categoryDto);
            return Ok(categoryDto);
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var categoryDto = await _categoryService.GetCategoryById(id);

            if (categoryDto == null)
                return NotFound("Category not found");

            await _categoryService.RemoveCategory(id);

            return NoContent();
        }
    }
}
