using System.Text;
using Application.Files.Interfaces;
using Application.Files.Services;
using Application.Repositories;
using Domain.Models.Products;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Moq;

namespace MultiMerk.Application.Tests.Files.Tests.Services.Test;
public class CsvServiceTest
{
    private readonly Mock<IFileParser> _fileParserMock;
    private readonly Mock<IProductRepository> _productRepoMock;
    private readonly CsvService _service;

    public CsvServiceTest()
    {
        _fileParserMock = new Mock<IFileParser>();
        _productRepoMock = new Mock<IProductRepository>();

        _service = new CsvService(_fileParserMock.Object);
        typeof(CsvService)
            .GetField("_productRepository", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)!
            .SetValue(_service, _productRepoMock.Object);
    }

    [Fact]
    public async Task UploadCsv_ReturnsFail_WhenFileExtensionIsInvalid()
    {
        var file = CreateMockFormFile("data", "invalid.txt"); // not in allowed list

        var result = await _service.UploadCsv(file);

        Assert.False(result.Success);
        Assert.Equal("Invalid file extension.", result.Message);
    }

    [Fact]
    public async Task UploadCsv_ReturnsFail_WhenExtensionNotCsv()
    {
        var file = CreateMockFormFile("data", "notcsv.xls"); // allowed, but not .csv

        var result = await _service.UploadCsv(file);

        Assert.False(result.Success);
        Assert.Equal("Invalid file extension.", result.Message);
    }

    [Fact]
    public async Task UploadCsv_ReturnsFail_WhenNoProductsFound()
    {
        var file = CreateMockFormFile("header1,header2", "valid.csv");

        _fileParserMock.Setup(p => p.GetDelimiterFromCsv(file)).Returns(',');
        _fileParserMock.Setup(p => p.GetProductsFromCsv(file, ',')).Returns(new List<Product>());

        var result = await _service.UploadCsv(file);

        Assert.False(result.Success);
        Assert.Equal("No products found in the CSV file.", result.Message);
    }


    [Fact]
    public async Task UploadCsv_ReturnsSuccess_WhenValidCsvUploaded()
    {
        var file = CreateMockFormFile("sku\n12345", "valid.csv");
        var products = new List<Product> { new Product("LC01-100-200-5") };

        _fileParserMock.Setup(p => p.GetDelimiterFromCsv(file)).Returns(',');
        _fileParserMock.Setup(p => p.GetProductsFromCsv(file, ',')).Returns(products);
        _productRepoMock.Setup(r => r.AddRangeAsync(products)).Returns(Task.CompletedTask);

        var result = await _service.UploadCsv(file);

        Assert.True(result.Success);
        Assert.Null(result.Message);
    }

    private IFormFile CreateMockFormFile(string content, string fileName)
    {
        var bytes = Encoding.UTF8.GetBytes(content);
        var stream = new MemoryStream(bytes);
        return new FormFile(stream, 0, stream.Length, "file", fileName);
    }


}

