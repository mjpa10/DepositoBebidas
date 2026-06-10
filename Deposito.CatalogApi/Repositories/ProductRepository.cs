using Deposito.CatalogApi.Context;
using Deposito.CatalogApi.Models;
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
        return await _context.Products.AsNoTracking().Include(p => p.Category).ToListAsync();
    }

    public async Task<Product> GetById(int id)
    {
        return await _context.Products.Include(p => p.Category).AsNoTracking().FirstOrDefaultAsync(p => p.ProductId == id);
    }

    public async Task<IEnumerable<Product>> GetByCategory(int id)
    {
        return await _context.Products.Include(c => c.Category).Where(p => p.CategoryId == id).AsNoTracking().ToListAsync();
    }
    
    public async Task<Product> GetByName(string name)
    {
        return await _context.Products.Include(p => p.Category).AsNoTracking().FirstOrDefaultAsync(p => p.Name == name);
    }

    public async Task<Product> Create(Product product)
    {
        _context.Products.Add(product);
        await _context.SaveChangesAsync();
        return product;
    }

    public async Task<Product> Update(Product product)
    {
        _context.Products.Update(product);
        await _context.SaveChangesAsync();
        return product;
    }

    public async Task<Product?> Delete(int id)
    {
        var product = await _context.Products.FirstOrDefaultAsync(p => p.ProductId == id);


        if (product == null)
            return null;

        _context.Products.Remove(product);
        await _context.SaveChangesAsync();
        return product;
    }
}
