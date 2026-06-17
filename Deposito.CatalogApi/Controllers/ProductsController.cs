using Deposito.CatalogApi.DTOs;
using Deposito.CatalogApi.Models;
using Deposito.CatalogApi.Pagination;
using Deposito.CatalogApi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Deposito.CatalogApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> Get()
        {
            var products = await _productService.GetProducts();
            return Ok(products);
        }

        [HttpGet("{id:int}", Name = "GetProduct")]
        public async Task<ActionResult<ProductDTO>> GetById(int id)
        {
            var product = await _productService.GetProductById(id);
            if (product == null)
            {
                return NotFound($"Produto com ID {id} não encontrado.");
            }
            return Ok(product);
        }

        [HttpGet("category/{id:int}")]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetByCategory(int id)
        {
            var products = await _productService.GetProductsByCategory(id);
            if (products == null) return NotFound($"Nenhum produto encontrado para a categoria com ID {id}.");

            return Ok(products);
        }

        //[HttpGet("search")]
        //public async Task<ActionResult<ProductDTO>> GetByName([FromQuery] string name)
        //{
        //    var product = await _productService.GetProductByName(name);
        //    if (product == null)
        //    {
        //        return NotFound(new { message = $"Produto com o nome '{name}' não encontrado." });
        //    }
        //    return Ok(product);
        //}

        [HttpGet("pagination")]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetProducts([FromQuery] ProductsParameters productsParams)
        {
            var products = await _productService.GetProducts(productsParams);

            var metadata = new
            {
                products.TotalCount,
                products.PageSize,
                products.CurrentPage,
                products.TotalPages,
                products.HasNext,
                products.HasPrevious
            };
            Response.Headers.Add("X-Pagination", System.Text.Json.JsonSerializer.Serialize(metadata));
            return Ok(products);
        }

        [HttpGet("filter")]
        public async Task<ActionResult<PagedList<ProductDTO>>> FilterProducts([FromQuery] ProductsFilter filter)
        {
            var products = await _productService.FilterProducts(filter);

            var metadata = new
            {
                products.TotalCount,
                products.PageSize,
                products.CurrentPage,
                products.TotalPages,
                products.HasNext,
                products.HasPrevious
            };
            Response.Headers.Add("X-Pagination", System.Text.Json.JsonSerializer.Serialize(metadata));

            return Ok(products);
        }

        [HttpPost]
        public async Task<ActionResult<ProductDTO>> Post([FromBody] ProductDTO productDto)
        {
            if (productDto == null) return BadRequest("Dados inválidos.");

            var createdProduct = await _productService.AddProduct(productDto);
            
            if (createdProduct is null) return BadRequest($"Category com ID {productDto.CategoryId} não encontrada.");

            return new CreatedAtRouteResult("GetProduct", new { id = createdProduct.ProductId },
            createdProduct);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int id, [FromBody] ProductDTO productDto)
        {
            if (id != productDto.ProductId)
                return BadRequest("O ID do caminho não corresponde ao ID do produto enviado.");

            if (productDto == null)
                return BadRequest("Dados inválidos.");

            await _productService.UpdateProduct(productDto);
            return Ok(productDto);
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var product = await _productService.GetProductById(id);
            if (product == null)
                return NotFound($"Produto com ID {id} não encontrado.");

            await _productService.RemoveProduct(id);
            return NoContent();
        }

    }
}
