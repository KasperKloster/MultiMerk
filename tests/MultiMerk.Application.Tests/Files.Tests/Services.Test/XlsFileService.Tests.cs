using System.Text;
using Application.Files.Interfaces;
using Application.Files.Services;
using Application.Repositories;
using Domain.Models.Products;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Moq;

namespace MultiMerk.Application.Tests.Files.Tests.Services.Test;

public class XlsFileServiceTests
{
    private readonly Mock<IFileParser> _mockParser;
    private readonly Mock<IProductRepository> _mockRepo;

    public XlsFileServiceTests()
    {
        _mockParser = new Mock<IFileParser>();
        _mockRepo = new Mock<IProductRepository>();
    }

    [Fact]
    public async Task CreateWeeklist_ShouldFail_WhenFileExtensionIsInvalid()
    {
        // Arrange
        var service = new XlsFileService(_mockParser.Object, _mockRepo.Object);
        var invalidFile = CreateMockFile("test.csv");

        // Act
        var result = await service.CreateWeeklist(invalidFile);

        // Assert
        Assert.False(result.Success);
        Assert.Equal("Invalid file extension.", result.Message);
        _mockRepo.Verify(r => r.AddRangeAsync(It.IsAny<List<Product>>()), Times.Never);
    }

    [Fact]
    public async Task CreateWeeklist_ShouldFail_WhenNoProductsFound()
    {
        // Arrange
        _mockParser.Setup(p => p.GetProductsFromXls(It.IsAny<IFormFile>()))
                   .Returns(new List<Product>());

        var service = new XlsFileService(_mockParser.Object, _mockRepo.Object);
        var file = CreateMockFile("test.xls");

        // Act
        var result = await service.CreateWeeklist(file);

        // Assert
        Assert.False(result.Success);
        Assert.Equal("No products found in the file.", result.Message);
        _mockRepo.Verify(r => r.AddRangeAsync(It.IsAny<List<Product>>()), Times.Never);
    }

    [Fact]
    public async Task CreateWeeklist_ShouldSucceed_WhenValidProductsAreParsed()
    {
        // Arrange
        var products = new List<Product> { new("123"), new("456") };

        _mockParser.Setup(p => p.GetProductsFromXls(It.IsAny<IFormFile>()))
                   .Returns(products);

        _mockRepo.Setup(r => r.AddRangeAsync(It.IsAny<List<Product>>()))
                 .Returns(Task.CompletedTask)
                 .Verifiable();

        var service = new XlsFileService(_mockParser.Object, _mockRepo.Object);
        var file = CreateMockFile("test.xls");

        // Act
        var result = await service.CreateWeeklist(file);

        // Assert
        Assert.True(result.Success);
        Assert.Null(result.Message);
        _mockRepo.Verify(r => r.AddRangeAsync(products), Times.Once);
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

}
