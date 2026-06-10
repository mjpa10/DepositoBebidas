using Deposito.CatalogApi.Context;
using Deposito.CatalogApi.Models;
using Microsoft.EntityFrameworkCore;

namespace Deposito.CatalogApi.Repositories;

public class CategoryRepository : ICategoryRepository
{
    private readonly AppDbContext _context;

    public CategoryRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Category>> GetAll()
    {
        return await _context.Categories.AsNoTracking().ToListAsync();
    }

    public async Task<Category?> GetById(int id)
    {
        return await _context.Categories.AsNoTracking().FirstOrDefaultAsync(c => c.CategoryId == id);
    }
    public async Task<Category?> GetByIdWithProducts(int id)
    {
        return await _context.Categories.AsNoTracking().Include(c => c.Products).FirstOrDefaultAsync(c => c.CategoryId == id);
    }

    public async Task<Category> Create(Category category)
    {
        _context.Categories.Add(category);
        await _context.SaveChangesAsync();
        return category;
    }

    public async Task<Category> Update(Category category)
    {
        _context.Categories.Update(category);
        await _context.SaveChangesAsync();
        return category;
    }

    public async Task<Category?> Delete(int id)
    {
        var category = await _context.Categories
            .FirstOrDefaultAsync(c => c.CategoryId == id);

        if (category == null)
            return null;

        _context.Categories.Remove(category);
        await _context.SaveChangesAsync();

        return category;
    }
    
}
