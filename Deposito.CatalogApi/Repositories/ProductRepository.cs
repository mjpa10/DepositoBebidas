using Deposito.CatalogApi.Context;
using Deposito.CatalogApi.Models;
using Deposito.CatalogApi.Pagination;
using Microsoft.EntityFrameworkCore;

namespace Deposito.CatalogApi.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly AppDbContext _context;   

    public ProductRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Product>> GetAll()
    {
        return await _context.Products.AsNoTracking().Include(c => c.Category).ToListAsync();
    }

    public async Task<IEnumerable<Product>> GetByCategory(int id)
    {
        return await _context.Products.AsNoTracking().Where(p => p.CategoryId == id)
            .Include(c => c.Category).ToListAsync();
    }

    public async Task<Product> GetById(int id)
    {
        return await _context.Products.AsNoTracking().Include(c => c.Category).FirstOrDefaultAsync(p => p.ProductId == id);
    }

    public async Task<IEnumerable<Product>> GetByName(string name)
    {
        return await _context.Products.AsNoTracking().Where(c => c.Name.Contains(name)).ToListAsync();
    }
    public async Task<PagedList<Product>> GetProducts(ProductsParameters productsParams)
    {
        var products = _context.Products
            .AsNoTracking()
            .Include(c => c.Category)
            .OrderBy(p => p.ProductId)
            .AsQueryable();
    
        return await PagedList<Product>.ToPagedList(products, productsParams.PageNumber, productsParams.PageSize);
    }
    public async Task<PagedList<Product>> FilterProducts(ProductsFilter filter)
    {
        var query = _context.Products
        .AsNoTracking()
        .Include(p => p.Category)
        .AsQueryable();

        if (!string.IsNullOrWhiteSpace(filter.Name))
        {
            query = query.Where(p =>
                p.Name.Contains(filter.Name));
        }

        if (filter.CategoryId.HasValue)
        {
            query = query.Where(p =>
                p.CategoryId == filter.CategoryId.Value);
        }

        if (filter.LowStock == true)
        {
            query = query.Where(p =>
                p.StockQuantity < p.MinimumStock);
        }

        query = filter.SortBy?.ToLower() switch
        {
            "name" => query.OrderBy(p => p.Name),
            "saleprice" => query.OrderBy(p => p.SalePrice),
            "costprice" => query.OrderBy(p => p.CostPrice),
            "stock" => query.OrderBy(p => p.StockQuantity),
            _ => query.OrderBy(p => p.ProductId)
        };

        return await PagedList<Product>.ToPagedList(
        query,
        filter.PageNumber,
        filter.PageSize);
    }

    public async Task<Product> Create(Product product)
    {
        _context.Products.Add(product);
        await _context.SaveChangesAsync();
        return product;
    }

    public async Task<Product> Update(Product product)
    {
        _context.Entry(product).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return product;
    }

    public async Task<Product> Delete(int id)
    {
        var product = await _context.Products.FirstOrDefaultAsync(p => p.ProductId == id);
        if (product is null) return null;

        _context.Products.Remove(product);
        await _context.SaveChangesAsync();
        return product;
    }

   
}
