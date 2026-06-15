using AutoMapper;
using Deposito.CatalogApi.DTOs;
using Deposito.CatalogApi.Models;
using Deposito.CatalogApi.Pagination;
using Deposito.CatalogApi.Repositories;

namespace Deposito.CatalogApi.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IMapper _mapper;

    public ProductService(IProductRepository productRepository, IMapper mapper, ICategoryRepository categoryRepository)
    {
        _productRepository = productRepository;
        _mapper = mapper;
        _categoryRepository = categoryRepository;
    }

    public async Task<IEnumerable<ProductDTO>> GetProducts()
    {
        var products = await _productRepository.GetAll();
        return _mapper.Map<IEnumerable<ProductDTO>>(products);
    }

    public async Task<IEnumerable<ProductDTO?>> GetProductsByCategory(int categoryId)
    {
        var category = await _categoryRepository.GetById(categoryId);

        if (category == null)
            return null;

        var products = await _productRepository.GetByCategory(categoryId);

        return _mapper.Map<IEnumerable<ProductDTO>>(products);
    }

    public async Task<ProductDTO> GetProductById(int id)
    {
        var product = await _productRepository.GetById(id);
        return _mapper.Map<ProductDTO>(product);
    }

    public async Task<IEnumerable<ProductDTO>> GetProductByName(string name)
    {
        var products = await _productRepository.GetByName(name);

        return _mapper.Map<IEnumerable<ProductDTO>>(products);
    }
    public async Task<PagedList<ProductDTO>> GetProducts(ProductsParameters productsParams)
    {
        var products = await _productRepository.GetProducts(productsParams);

        var items = _mapper.Map<List<ProductDTO>>(products);

        return new PagedList<ProductDTO>(items, products.TotalCount, productsParams.PageNumber, productsParams.PageSize);
    }
    public async Task<ProductDTO> AddProduct(ProductDTO productDto)
    {
        var category = await _categoryRepository.GetById(productDto.CategoryId);

        if (category == null) return null;

        var product = _mapper.Map<Product>(productDto);
        await _productRepository.Create(product);
        productDto.ProductId = product.ProductId;
        return _mapper.Map<ProductDTO>(product);
    }

    public async Task UpdateProduct(ProductDTO productDto)
    {
        var product = _mapper.Map<Product>(productDto);
        await _productRepository.Update(product);
    }

    public async Task RemoveProduct(int id)
    {
        await _productRepository.Delete(id);
    }
}
