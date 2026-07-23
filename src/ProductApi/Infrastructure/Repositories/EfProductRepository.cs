using Microsoft.EntityFrameworkCore;
using ProductApi.Application.Contracts;
using ProductApi.Domain;
using ProductApi.Infrastructure.Persistence;

namespace ProductApi.Infrastructure.Repositories;

public sealed class EfProductRepository(ProductDbContext context) : IProductRepository
{
    private readonly ProductDbContext _context = context;

    public async Task<IReadOnlyList<Product>> GetAllAsync(CancellationToken cancellationToken = default)
        => await _context.Products.OrderBy(product => product.Name).ToListAsync(cancellationToken);

    public async Task<Product?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => await _context.Products.SingleOrDefaultAsync(product => product.Id == id, cancellationToken);

    public async Task<Product> AddAsync(Product product, CancellationToken cancellationToken = default)
    {
        await _context.Products.AddAsync(product, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return product;
    }

    public async Task<Product?> UpdateAsync(Product product, CancellationToken cancellationToken = default)
    {
        var existingProduct = await _context.Products.FindAsync([product.Id], cancellationToken);
        if (existingProduct is null)
        {
            return null;
        }

        existingProduct.Name = product.Name;
        existingProduct.Description = product.Description;
        existingProduct.Price = product.Price;
        existingProduct.Stock = product.Stock;
        await _context.SaveChangesAsync(cancellationToken);
        return existingProduct;
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var existingProduct = await _context.Products.FindAsync([id], cancellationToken);
        if (existingProduct is null)
        {
            return false;
        }

        _context.Products.Remove(existingProduct);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
}
