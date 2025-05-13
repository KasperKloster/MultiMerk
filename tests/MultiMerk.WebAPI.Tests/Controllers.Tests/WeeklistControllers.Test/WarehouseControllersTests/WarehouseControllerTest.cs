using Application.DTOs.Weeklists;
using Application.Files.Interfaces;
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
using WebAPI.Controllers.WeeklistControllers.WarehouseControllers;

namespace MultiMerk.WebAPI.Tests.Controllers.Tests.WeeklistControllers.Test.WarehouseControllersTests;

public class WarehouseControllerTests
{
    private readonly Mock<IProductService> _productServiceMock = new();
    private readonly Mock<IXlsFileService> _xlsFileServiceMock = new();
    private readonly Mock<IWeeklistService> _weeklistServiceMock = new();
    private readonly Mock<IWeeklistTaskLinkService> _weeklistTaskLinkServiceMock = new();

    private readonly WarehouseController _controller;

    public WarehouseControllerTests()
    {
        _controller = new WarehouseController(
            _weeklistServiceMock.Object,
            _weeklistTaskLinkServiceMock.Object,
            _productServiceMock.Object,
            _xlsFileServiceMock.Object);
    }

    [Fact]
    public async Task GetChecklist_ReturnsFile_WhenSuccessful()
    {
        // Arrange
        int weeklistId = 1;
        var products = new List<Product>();
        var xlsBytes = Encoding.UTF8.GetBytes("xls data");
        var weeklist = new WeeklistDto { Number = 123, OrderNumber = "A1", ShippingNumber = "S1" };

        _productServiceMock.Setup(s => s.GetProductsFromWeeklist(weeklistId)).ReturnsAsync(products);
        _xlsFileServiceMock.Setup(s => s.GetProductChecklist(products)).Returns(xlsBytes);
        _weeklistServiceMock.Setup(s => s.GetWeeklistAsync(weeklistId)).ReturnsAsync(weeklist);

        // Act
        var result = await _controller.GetChecklist(weeklistId);

        // Assert
        var fileResult = Assert.IsType<FileContentResult>(result);
        Assert.Equal("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileResult.ContentType);
        Assert.Equal(xlsBytes, fileResult.FileContents);
        Assert.Contains("Checklist.xls", fileResult.FileDownloadName);
    }

    [Fact]
    public async Task GetChecklist_Returns500_OnException()
    {
        _productServiceMock.Setup(s => s.GetProductsFromWeeklist(It.IsAny<int>()))
            .ThrowsAsync(new Exception("DB error"));

        var result = await _controller.GetChecklist(1);

        var objectResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(500, objectResult.StatusCode);
        Assert.Contains("Something went wrong", objectResult.Value!.ToString());
    }

    [Fact]
    public async Task UploadChecklist_ReturnsBadRequest_WhenFileFails()
    {
        var file = CreateMockFile("bad.xls");
        _productServiceMock.Setup(s => s.UpdateProductsFromFile(file))
            .ReturnsAsync(FilesResult.Fail("Invalid file"));

        var result = await _controller.UploadChecklist(file, 1);

        var badRequest = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Invalid file", badRequest.Value);
    }

    [Fact]
    public async Task UploadChecklist_ReturnsOk_WhenSuccessful()
    {
        var file = CreateMockFile("good.xls");
        _productServiceMock.Setup(s => s.UpdateProductsFromFile(file))
            .ReturnsAsync(FilesResult.SuccessResult());

        _weeklistTaskLinkServiceMock.Setup(s => s.UpdateTaskStatusAndAdvanceNext(
            1,
            WeeklistTaskNameEnum.CreateChecklist,
            WeeklistTaskNameEnum.InsertWarehouseList))
            .ReturnsAsync(OperationResult.Ok());

        var result = await _controller.UploadChecklist(file, 1);

        Assert.IsType<OkResult>(result);
    }

    [Fact]
    public async Task GetWarehouselist_ReturnsFile_WhenSuccessful()
    {
        var weeklistId = 1;
        var products = new List<Product>();
        var xlsBytes = Encoding.UTF8.GetBytes("xls");
        var weeklist = new WeeklistDto { Number = 456, OrderNumber = "ORD", ShippingNumber = "SHIP" };

        _productServiceMock.Setup(s => s.GetProductsFromWeeklist(weeklistId)).ReturnsAsync(products);
        _xlsFileServiceMock.Setup(s => s.GetProductWarehouselist(products)).Returns(xlsBytes);
        _weeklistServiceMock.Setup(s => s.GetWeeklistAsync(weeklistId)).ReturnsAsync(weeklist);

        var result = await _controller.GetWarehouselist(weeklistId);

        var fileResult = Assert.IsType<FileContentResult>(result);
        Assert.Equal(xlsBytes, fileResult.FileContents);
        Assert.Contains("Warehouselist.xls", fileResult.FileDownloadName);
    }

    [Fact]
    public async Task MarkTaskAsComplete_ReturnsOk_WhenSuccessful()
    {
        _weeklistTaskLinkServiceMock.Setup(s => s.UpdateTaskStatusAndAdvanceNext(
            1,
            WeeklistTaskNameEnum.InsertWarehouseList,
            WeeklistTaskNameEnum.ImportProductList))
            .ReturnsAsync(OperationResult.Ok());

        var result = await _controller.MarkTaskAsComplete(1);

        Assert.IsType<OkResult>(result);
    }

    private static IFormFile CreateMockFile(string fileName)
    {
        var stream = new MemoryStream(Encoding.UTF8.GetBytes("test"));
        return new FormFile(stream, 0, stream.Length, "file", fileName)
        {
            Headers = new HeaderDictionary(),
            ContentType = "application/vnd.ms-excel"
        };
    }
}