using ProductApi.Application.Contracts;
using ProductApi.Application.Services;
using ProductApi.Domain;

namespace ProductApi.Tests.Unit;

public sealed class ProductServiceTests
{
    [Fact]
    public async Task CreateAsync_ShouldStoreProduct()
    {
        var repository = new FakeProductRepository();
        var service = new ProductService(repository);

        var created = await service.CreateAsync(new Product
        {
            Name = "Keyboard",
            Description = "Mechanical",
            Price = 99.99m,
            Stock = 5
        });

        Assert.NotEqual(Guid.Empty, created.Id);
        Assert.Equal("Keyboard", created.Name);
        Assert.Single(repository.Products);
    }

    [Fact]
    public async Task UpdateAsync_ShouldReturnUpdatedProduct()
    {
        var repository = new FakeProductRepository();
        var service = new ProductService(repository);
        var existing = await service.CreateAsync(new Product
        {
            Name = "Mouse",
            Price = 29.50m,
            Stock = 10
        });

        var updated = await service.UpdateAsync(existing.Id, new Product
        {
            Name = "Mouse",
            Description = "Wireless",
            Price = 39.50m,
            Stock = 8
        });

        Assert.NotNull(updated);
        Assert.Equal("Wireless", updated!.Description);
        Assert.Equal(39.50m, updated.Price);
    }

    [Fact]
    public async Task DeleteAsync_ShouldRemoveExistingProduct()
    {
        var repository = new FakeProductRepository();
        var service = new ProductService(repository);
        var product = await service.CreateAsync(new Product
        {
            Name = "Monitor",
            Price = 220m,
            Stock = 3
        });

        var deleted = await service.DeleteAsync(product.Id);

        Assert.True(deleted);
        Assert.Empty(repository.Products);
    }

    private sealed class FakeProductRepository : IProductRepository
    {
        public List<Product> Products { get; } = [];

        public Task<IReadOnlyList<Product>> GetAllAsync(CancellationToken cancellationToken = default)
            => Task.FromResult<IReadOnlyList<Product>>(Products);

        public Task<Product?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
            => Task.FromResult(Products.FirstOrDefault(product => product.Id == id));

        public Task<Product> AddAsync(Product product, CancellationToken cancellationToken = default)
        {
            Products.Add(product);
            return Task.FromResult(product);
        }

        public Task<Product?> UpdateAsync(Product product, CancellationToken cancellationToken = default)
        {
            var existingProduct = Products.FirstOrDefault(item => item.Id == product.Id);
            if (existingProduct is null)
            {
                return Task.FromResult<Product?>(null);
            }

            existingProduct.Name = product.Name;
            existingProduct.Description = product.Description;
            existingProduct.Price = product.Price;
            existingProduct.Stock = product.Stock;
            return Task.FromResult<Product?>(existingProduct);
        }

        public Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var deleted = Products.RemoveAll(product => product.Id == id) > 0;
            return Task.FromResult(deleted);
        }
    }
}
