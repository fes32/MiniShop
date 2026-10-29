
using Microsoft.EntityFrameworkCore;
using MiniShop.Application.Interfaces;
using MiniShop.Domain.Entities;
using MiniShop.Infrastructure.Persistence;

namespace MiniShop.Infrastructure.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly AppDbContext _context;

    public ProductRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<Product>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Products.AsNoTracking().OrderBy(product => product.Id).ToListAsync(cancellationToken);
    }

    public async Task<Product?> GetByIdAsync(int id,CancellationToken cancellationToken = default)
    {
        return await _context.Products.FirstOrDefaultAsync(product => product.Id == id,cancellationToken);
    }

    public async Task AddAsync(Product product,CancellationToken cancellationToken = default)
    {
        await _context.Products.AddAsync(product, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(Product product, CancellationToken cancellationToken = default)
    {
        _context.Products.Update(product);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Product product,CancellationToken cancellationToken = default)
    {
        _context.Products.Remove(product);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<bool> ExistsAsync(int id,CancellationToken cancellationToken = default)
    {
        return await _context.Products.AnyAsync(product => product.Id == id, cancellationToken);
    }
}
