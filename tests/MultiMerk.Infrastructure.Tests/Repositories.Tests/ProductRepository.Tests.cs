using Domain.Entities.Products;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace MultiMerk.Infrastructure.Tests.Repositories.Tests;

public class ProductRepositoryTests
{

    private AppDbContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDb_" + System.Guid.NewGuid()) // Unique DB for each test
            .Options;

        return new AppDbContext(options);
    }

    
    [Fact]
    public async Task AddRangeAsync_AddsProductsToDatabase()
    {
        // Arrange
        var context = CreateDbContext();
        var repository = new ProductRepository(context);
        var products = new List<Product>
        {
            new Product("SKU001"),
            new Product("SKU002")
        };

        // Act
        await repository.AddRangeAsync(products);

        // Assert
        var dbProducts = context.Products.ToList();
        Assert.Equal(2, dbProducts.Count);
        Assert.Contains(dbProducts, p => p.Sku == "SKU001");
        Assert.Contains(dbProducts, p => p.Sku == "SKU002");
    }

    [Fact]
    public async Task AddRangeAsync_DoesNotThrow_WhenEmptyListProvided()
    {
        // Arrange
        var context = CreateDbContext();
        var repository = new ProductRepository(context);
        var emptyList = new List<Product>();

        // Act
        var exception = await Record.ExceptionAsync(() => repository.AddRangeAsync(emptyList));

        // Assert
        Assert.Null(exception); // Should not throw
    }

}
