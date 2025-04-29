using System.Text;
using Application.Files.Interfaces;
using Application.Files.Services;
using Application.Repositories;
using Application.Repositories.Weeklists;
using Domain.Entities.Products;
using Domain.Entities.Weeklists.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Moq;

namespace MultiMerk.Application.Tests.Files.Tests.Services.Test;

public class XlsFileServiceTests
{
    private readonly Mock<IFileParser> _mockParser = new();
    private readonly Mock<IProductRepository> _mockProductRepo = new();
    private readonly Mock<IWeeklistRepository> _mockWeeklistRepo = new();
    private readonly Mock<IWeeklistTaskRepository> _mockWeeklistTaskRepository = new();
    private readonly Mock<IWeeklistTaskLinkRepository> _mockWeeklistTaskLinkRepository = new();

    [Fact]
    public async Task GetProductsFromXls_ShouldFail_WhenFileExtensionIsInvalid()
    {
        // Arrange
        var service = new XlsFileService(_mockParser.Object);
        var invalidFile = CreateMockFile("invalid.csv");

        // Act
        var result = await service.GetProductsFromXls(invalidFile);

        // Assert
        Assert.False(result.Success);
        Assert.Equal("Invalid file extension.", result.Message);
    }

    [Fact]
    public async Task GetProductsFromXls_ShouldFail_WhenNoProductsFound()
    {
        // Arrange
        var service = new XlsFileService(_mockParser.Object);
        _mockParser.Setup(p => p.GetProductsFromXls(It.IsAny<IFormFile>())).Returns(new List<Product>());
        var file = CreateMockFile("valid.xls");

        // Act
        var result = await service.GetProductsFromXls(file);

        // Assert
        Assert.False(result.Success);
        Assert.Equal("No products found in the file.", result.Message);
    }

    [Fact]
    public async Task GetProductsFromXls_ShouldSucceed_WhenProductsAreReturned()
    {
        // Arrange
        var expectedProducts = new List<Product> { new("123"), new("456") };
        _mockParser.Setup(p => p.GetProductsFromXls(It.IsAny<IFormFile>())).Returns(expectedProducts);
        var service = new XlsFileService(_mockParser.Object);
        var file = CreateMockFile("valid.xls");

        // Act
        var result = await service.GetProductsFromXls(file);

        // Assert
        Assert.True(result.Success);
        Assert.Equal(expectedProducts, result.Products);
        Assert.Null(result.Message);
    }


    private IFormFile CreateMockFile(string name = "test.xls")
    {
        var content = "dummy content";
        var stream = new MemoryStream(Encoding.UTF8.GetBytes(content));
        return new FormFile(stream, 0, stream.Length, "file", name)
        {
            Headers = new HeaderDictionary(),
            ContentType = "application/vnd.ms-excel"
        };
    }

    private static Weeklist CreateMockWeeklist()
    {
        return new Weeklist
        {
            Id = 1,
            Number = 568,
            OrderNumber = "EX123456",
            Supplier = "TVC"
        };
    }
}