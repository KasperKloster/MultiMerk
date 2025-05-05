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

    [Fact]
    public async Task GetProductsReadyForAI_ReturnsOnlyProductsWithoutTemplate_ForGivenWeeklistId()
    {
        // Arrange
        var context = CreateDbContext();
        var repository = new ProductRepository(context);

        var products = new List<Product>
        {
            new Product("SKU1") { WeeklistId = 1, TemplateId = null },
            new Product("SKU2") { WeeklistId = 1, TemplateId = 123 },
            new Product("SKU3") { WeeklistId = 1, TemplateId = null },
            new Product("SKU4") { WeeklistId = 2, TemplateId = null }

        };

        context.Products.AddRange(products);
        await context.SaveChangesAsync();

        // Act
        var result = await repository.GetProductsReadyForAI(1);

        // Assert        
        Assert.Equal(2, result.Count); // Expects to return 2
        Assert.All(result, p => Assert.Null(p.TemplateId)); // Expects to return None products with template ID        
    }

    [Fact]
    public async Task GetProductsReadyForAI_ReturnsDistinctSeriesAndNoTemplates()
    {
        // Arrange
        var context = CreateDbContext();
        var repository = new ProductRepository(context);

        var products = new List<Product>
        {
            // Series group with no TemplateId — only one should be returned
            new Product("SKU-A1") { WeeklistId = 1, TemplateId = null, Series = "SERIES-A" },
            new Product("SKU-A2") { WeeklistId = 1, TemplateId = null, Series = "SERIES-A" },
            new Product("SKU-A3") { WeeklistId = 1, TemplateId = null, Series = "SERIES-A" },

            // Product with Series and TemplateId — should be excluded
            new Product("SKU-B1") { WeeklistId = 1, TemplateId = 42, Series = "SERIES-B" },

            // Series group with mixed TemplateId — valid one should still be picked
            new Product("SKU-C1") { WeeklistId = 1, TemplateId = 99, Series = "SERIES-C" },
            new Product("SKU-C2") { WeeklistId = 1, TemplateId = null, Series = "SERIES-C" },

            // Product with no Series and no Template — should be included
            new Product("SKU-N1") { WeeklistId = 1, TemplateId = null, Series = null },

            // Product from a different weeklist — should be excluded
            new Product("SKU-X1") { WeeklistId = 2, TemplateId = null, Series = "SERIES-X" },
        };

        context.Products.AddRange(products);
        await context.SaveChangesAsync();

        // Act
        var result = await repository.GetProductsReadyForAI(1);

        // Assert
        Assert.Equal(3, result.Count); // Expect: one from SERIES-A, one from SERIES-C, one with null series
        Assert.All(result, p => Assert.Null(p.TemplateId)); // No templates
        Assert.Contains(result, p => p.Series == "SERIES-A");
        Assert.Contains(result, p => p.Series == "SERIES-C");
        Assert.Contains(result, p => p.Series == null);
    }

}
