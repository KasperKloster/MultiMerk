using Application.DTOs.Weeklists;
using Application.Services.Interfaces.Files.csv;
using Application.Services.Interfaces.Tasks;
using Application.Services.Interfaces.Weeklists;
using Domain.Common;
using Domain.Entities.Files;
using Domain.Entities.Products;
using Domain.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Text;
using WebAPI.Controllers.WeeklistControllers.ContentControllers;

namespace MultiMerk.WebAPI.Tests.Controllers.Tests.WeeklistControllers.ContentControllers;

public class ContentControllerTest
{
    private readonly Mock<IWeeklistService> _weeklistServiceMock;
    private readonly Mock<IWeeklistTaskLinkService> _taskLinkServiceMock;
    private readonly Mock<IContentService> _contentServiceMock;
    private readonly Mock<IAICsvService> _aiCsvServiceMock;
    private readonly ContentController _controller;

    public ContentControllerTest()
    {
        _weeklistServiceMock = new Mock<IWeeklistService>();
        _taskLinkServiceMock = new Mock<IWeeklistTaskLinkService>();
        _contentServiceMock = new Mock<IContentService>();
        _aiCsvServiceMock = new Mock<IAICsvService>();

        _controller = new ContentController(
            _weeklistServiceMock.Object,
            _taskLinkServiceMock.Object,
            _contentServiceMock.Object,
            _aiCsvServiceMock.Object
        );
    }

    [Fact]
    public async Task GetProductsReadyForAIContent_ReturnsFileResult_WhenSuccessful()
    {
        var weeklistId = 1;
        var products = new List<Product> { new Product(sku: "LC10-1001-1-123") { Id = 10 } };
        var csvBytes = Encoding.UTF8.GetBytes("mock csv");
        var weeklistDto = new WeeklistDto { Number = 2025, OrderNumber = "ORD123", ShippingNumber = "SHIP123" };

        _contentServiceMock.Setup(s => s.GetProductsReadyForAI(weeklistId)).ReturnsAsync(products);
        _aiCsvServiceMock.Setup(s => s.GenerateProductsReadyForAICSV(products)).Returns(csvBytes);
        _weeklistServiceMock.Setup(s => s.GetWeeklistAsync(weeklistId)).ReturnsAsync(weeklistDto);
        _taskLinkServiceMock
            .Setup(s => s.UpdateTaskStatus(It.IsAny<int>(), It.IsAny<WeeklistTaskNameEnum>(), It.IsAny<WeeklistTaskStatusEnum>()))
            .ReturnsAsync(OperationResult.Ok());

        var result = await _controller.GetProductsReadyForAIContent(weeklistId);

        var fileResult = Assert.IsType<FileContentResult>(result);
        Assert.Equal("text/csv", fileResult.ContentType);
        Assert.Equal(csvBytes, fileResult.FileContents);
        Assert.Contains("Ready-For-AI.csv", fileResult.FileDownloadName);
    }

    [Fact]
    public async Task UploadAIContent_ReturnsOk_WhenSuccessful()
    {
        var mockFile = CreateMockFile("content.csv", "some,content");
        var mockProducts = new List<Product> { new Product(sku: "LC10-1001-1-123") { Id = 1 } };
        var fileResult = FilesResult.SuccessResultWithProducts(mockProducts);

        _aiCsvServiceMock.Setup(s => s.GetProductsFromAI(mockFile)).Returns(fileResult);
        _contentServiceMock.Setup(s => s.InsertAIProductContent(mockProducts)).Returns(FilesResult.SuccessResult);

        var result = await _controller.UploadAIContent(mockFile, 123);

        Assert.IsType<OkResult>(result);
    }

    [Fact]
    public async Task UploadAIContent_ReturnsBadRequest_WhenCsvFails()
    {
        var mockFile = CreateMockFile("bad.csv", "bad,data");
        var failResult = FilesResult.Fail("Invalid CSV format");

        _aiCsvServiceMock.Setup(s => s.GetProductsFromAI(mockFile)).Returns(failResult);

        var result = await _controller.UploadAIContent(mockFile, 123);

        var badRequest = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Invalid CSV format", badRequest.Value);
    }

    private static IFormFile CreateMockFile(string fileName, string content)
    {
        var stream = new MemoryStream(Encoding.UTF8.GetBytes(content));
        return new FormFile(stream, 0, stream.Length, "file", fileName)
        {
            Headers = new HeaderDictionary(),
            ContentType = "text/csv"
        };
    }
}