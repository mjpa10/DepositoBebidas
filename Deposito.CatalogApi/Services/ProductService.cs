using AutoMapper;
using Deposito.CatalogApi.DTOs;
using Deposito.CatalogApi.Models;
using Deposito.CatalogApi.Repositories;

namespace Deposito.CatalogApi.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;

    public ProductService(IProductRepository productRepository, IMapper mapper)
    {
        _productRepository = productRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ProductDTO>> GetProducts()
    {
        var products = await _productRepository.GetAll();
        return _mapper.Map<IEnumerable<ProductDTO>>(products);
    }

    public async Task<IEnumerable<ProductDTO>> GetProductsByCategory(int id)
    {
        var products = await _productRepository.GetByCategory(id);
        return _mapper.Map<IEnumerable<ProductDTO>>(products);
    }

    public async Task<ProductDTO> GetProductById(int id)
    {
        var product = await _productRepository.GetById(id);
        return _mapper.Map<ProductDTO>(product);
    }

    public async Task<ProductDTO> GetProductByName(string name)
    {
        var product = await _productRepository.GetByName(name);
        return _mapper.Map<ProductDTO>(product);
    }
    public async Task<ProductDTO> AddProduct(ProductDTO productDto)
    {
        var product = _mapper.Map<Product>(productDto);
        await _productRepository.Create(product);
        //productDto.ProductId = product.ProductId;
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
