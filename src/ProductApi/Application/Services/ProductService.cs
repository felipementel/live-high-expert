using ProductApi.Application.Contracts;
using ProductApi.Domain;

namespace ProductApi.Application.Services;

public sealed class ProductService(IProductRepository repository) : IProductService
{
    private readonly IProductRepository _repository = repository;

    public Task<IReadOnlyList<Product>> GetAllAsync(CancellationToken cancellationToken = default)
        => _repository.GetAllAsync(cancellationToken);

    public Task<Product?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => _repository.GetByIdAsync(id, cancellationToken);

    public Task<Product> CreateAsync(Product product, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(product.Name))
        {
            throw new ArgumentException("Product name is required.", nameof(product));
        }

        if (product.Price < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(product), "Price cannot be negative.");
        }

        if (product.Stock < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(product), "Stock cannot be negative.");
        }

        product.Id = Guid.NewGuid();
        product.CreatedAt = DateTime.UtcNow;
        return _repository.AddAsync(product, cancellationToken);
    }

    public Task<Product?> UpdateAsync(Guid id, Product product, CancellationToken cancellationToken = default)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException("Product id is required.", nameof(id));
        }

        if (string.IsNullOrWhiteSpace(product.Name))
        {
            throw new ArgumentException("Product name is required.", nameof(product));
        }

        if (product.Price < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(product), "Price cannot be negative.");
        }

        if (product.Stock < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(product), "Stock cannot be negative.");
        }

        product.Id = id;
        return _repository.UpdateAsync(product, cancellationToken);
    }

    public Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        => _repository.DeleteAsync(id, cancellationToken);
}
