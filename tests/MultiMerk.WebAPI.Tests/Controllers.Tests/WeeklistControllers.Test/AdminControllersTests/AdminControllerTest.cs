using Application.DTOs.Weeklists;
using Application.Services.Interfaces.Files;
using Application.Services.Interfaces.Products;
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
using WebAPI.Controllers.WeeklistControllers.AdminControllers;

namespace MultiMerk.WebAPI.Tests.Controllers.Tests.WeeklistControllers.Test.AdminControllersTests;

public class AdminControllerTests
{
    private readonly Mock<IProductService> _productServiceMock;
    private readonly Mock<IWeeklistService> _weeklistServiceMock;
    private readonly Mock<IWeeklistTaskLinkService> _taskLinkServiceMock;
    private readonly Mock<IZipService> _zipServiceMock;
    private readonly AdminController _controller;

    public AdminControllerTests()
    {
        _productServiceMock = new Mock<IProductService>();
        _weeklistServiceMock = new Mock<IWeeklistService>();
        _taskLinkServiceMock = new Mock<IWeeklistTaskLinkService>();
        _zipServiceMock = new Mock<IZipService>();

        _controller = new AdminController(
            _productServiceMock.Object,
            _weeklistServiceMock.Object,
            _taskLinkServiceMock.Object,
            _zipServiceMock.Object
        );
    }

    [Fact]
    public async Task AssignEan_ReturnsOk_WhenSuccess()
    {
        var mockFile = CreateMockFile("ean.xls", "ean content");
        var weeklistId = 1;
        var result = FilesResult.SuccessResult();

        _productServiceMock.Setup(x => x.UpdateProductsFromFile(mockFile)).ReturnsAsync(result);
        _taskLinkServiceMock.Setup(x => x.UpdateTaskStatus(weeklistId, WeeklistTaskNameEnum.AssignEAN, WeeklistTaskStatusEnum.Done)).ReturnsAsync(OperationResult.Ok()); 
        var response = await _controller.AssignEan(mockFile, weeklistId);
        Assert.IsType<OkResult>(response);
    }

    [Fact]
    public async Task AssignEan_ReturnsBadRequest_WhenFailed()
    {
        var mockFile = CreateMockFile("ean.xls", "ean content");
        var result = FilesResult.Fail("Invalid file");

        _productServiceMock.Setup(x => x.UpdateProductsFromFile(mockFile)).ReturnsAsync(result);

        var response = await _controller.AssignEan(mockFile, 1);

        var badRequest = Assert.IsType<BadRequestObjectResult>(response);
        Assert.Equal("Invalid file", badRequest.Value);
    }

    [Fact]
    public async Task UploadOutOfStock_ReturnsOk_WhenSuccess()
    {
        var mockFile = CreateMockFile("outofstock.xls", "stock content");
        var stockProducts = new Dictionary<string, int> { { "SKU1", 10 } };
        var initialResult = FilesResult.SuccessResultWithOutOfStock(stockProducts);
        var updateResult = FilesResult.SuccessResult();

        _productServiceMock.Setup(x => x.GetProductsFromOutOfStock(mockFile)).Returns(initialResult);
        _productServiceMock.Setup(x => x.UpdateProductsFromStockProducts(stockProducts)).ReturnsAsync(updateResult);
        _taskLinkServiceMock.Setup(x =>
            x.UpdateTaskStatus(It.IsAny<int>(), WeeklistTaskNameEnum.InsertOutOfStock, WeeklistTaskStatusEnum.Done))
            .ReturnsAsync(OperationResult.Ok());

        var response = await _controller.UploadOutOfStock(mockFile, 5);

        Assert.IsType<OkResult>(response);
    }

    [Fact]
    public async Task ImportProductList_ReturnsZipFile_WhenSuccess()
    {
        var weeklistId = 1;
        var weeklistDto = new WeeklistDto { Number = 2025 };
        var products = new List<Product> { new Product(sku: "LC01-1001-1001-1") { Id = 1 } };
        var zipBytes = Encoding.UTF8.GetBytes("zipcontent");

        _weeklistServiceMock.Setup(x => x.GetWeeklistAsync(weeklistId)).ReturnsAsync(weeklistDto);
        _productServiceMock.Setup(x => x.GetProductsFromWeeklist(weeklistId)).ReturnsAsync(products);
        _zipServiceMock.Setup(x => x.CreateZipAdminImportAsync(weeklistDto, products)).ReturnsAsync(zipBytes);

        var result = await _controller.ImportProductList(weeklistId);

        var fileResult = Assert.IsType<FileContentResult>(result);
        Assert.Equal("application/zip", fileResult.ContentType);
        Assert.Equal("2025-Admin.zip", fileResult.FileDownloadName);
        Assert.Equal(zipBytes, fileResult.FileContents);
    }

    private static IFormFile CreateMockFile(string name, string content)
    {
        var stream = new MemoryStream(Encoding.UTF8.GetBytes(content));
        return new FormFile(stream, 0, stream.Length, "file", name)
        {
            Headers = new HeaderDictionary(),
            ContentType = "application/vnd.ms-excel"
        };
    }
}