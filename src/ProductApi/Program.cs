using Microsoft.EntityFrameworkCore;
using ProductApi.Application.Contracts;
using ProductApi.Application.Services;
using ProductApi.Contracts;
using ProductApi.Domain;
using ProductApi.Infrastructure.Persistence;
using ProductApi.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi();

builder.Services.AddDbContext<ProductDbContext>(options =>
    options.UseInMemoryDatabase("products-db"));

builder.Services.AddScoped<IProductRepository, EfProductRepository>();
builder.Services.AddScoped<IProductService, ProductService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.MapGet("/health", () => Results.Ok(new { status = "Healthy" }));

var products = app.MapGroup("/products");

products.MapGet("/", async (IProductService productService, CancellationToken cancellationToken) =>
    Results.Ok(await productService.GetAllAsync(cancellationToken)));

products.MapGet("/{id:guid}", async (Guid id, IProductService productService, CancellationToken cancellationToken) =>
{
    var product = await productService.GetByIdAsync(id, cancellationToken);
    return product is null ? Results.NotFound() : Results.Ok(product);
});

products.MapPost("/", async (ProductRequest request, IProductService productService, CancellationToken cancellationToken) =>
{
    var product = new Product
    {
        Name = request.Name,
        Description = request.Description,
        Price = request.Price,
        Stock = request.Stock
    };

    var createdProduct = await productService.CreateAsync(product, cancellationToken);
    return Results.Created($"/products/{createdProduct.Id}", createdProduct);
});

products.MapPut("/{id:guid}", async (Guid id, ProductRequest request, IProductService productService, CancellationToken cancellationToken) =>
{
    var updatedProduct = await productService.UpdateAsync(id, new Product
    {
        Name = request.Name,
        Description = request.Description,
        Price = request.Price,
        Stock = request.Stock
    }, cancellationToken);

    return updatedProduct is null ? Results.NotFound() : Results.Ok(updatedProduct);
});

products.MapDelete("/{id:guid}", async (Guid id, IProductService productService, CancellationToken cancellationToken) =>
{
    var deleted = await productService.DeleteAsync(id, cancellationToken);
    return deleted ? Results.NoContent() : Results.NotFound();
});

app.Run();

public partial class Program;
