using System.Text;
using Application.DTOs.Weeklists;
using Application.Services.Interfaces.Weeklists;
using Domain.Entities.Files;
using Domain.Entities.Weeklists.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using WebAPI.Controllers.WeeklistControllers;

namespace MultiMerk.WebAPI.Tests.Controllers.Tests.WeeklistControllers.Test;

public class WeeklistControllerTest
{
    private readonly Mock<IWeeklistService> _weeklistServiceMock;
    private readonly WeeklistController _weeklistController;

    public WeeklistControllerTest()
    {
        _weeklistServiceMock = new Mock<IWeeklistService>();
        _weeklistController = new WeeklistController(_weeklistServiceMock.Object);
    }

    [Fact]
    public async Task CreateWeeklist_ReturnsOk_WhenUploadIsSuccessful()
    {
        var fileMock = CreateMockXlsFile("test.xls");
        var weeklistMock = CreateMockWeeklist();

        _weeklistServiceMock
            .Setup(s => s.CreateWeeklist(It.IsAny<IFormFile>(), It.IsAny<Weeklist>()))
            .ReturnsAsync(FilesResult.SuccessResult());

        var result = await _weeklistController.CreateWeeklist(fileMock, weeklistMock.Number, weeklistMock.OrderNumber, weeklistMock.Supplier, weeklistMock.ShippingNumber);

        var okResult = Assert.IsType<OkResult>(result);
        Assert.Equal(200, okResult.StatusCode);
    }

    [Fact]
    public async Task CreateWeeklist_ReturnsBadRequest_WhenUploadFails()
    {
        var fileMock = CreateMockXlsFile("bad.xls");
        var weeklistMock = CreateMockWeeklist();

        _weeklistServiceMock
            .Setup(s => s.CreateWeeklist(It.IsAny<IFormFile>(), It.IsAny<Weeklist>()))
            .ReturnsAsync(FilesResult.Fail("Upload failed"));

        var result = await _weeklistController.CreateWeeklist(fileMock, weeklistMock.Number, weeklistMock.OrderNumber, weeklistMock.Supplier, weeklistMock.ShippingNumber);

        var badRequest = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal(400, badRequest.StatusCode);
        Assert.Equal("Upload failed", badRequest.Value);
    }

    [Fact]
    public async Task GetAllWeeklists_ReturnsOk_WithData()
    {
        var mockWeeklists = new List<WeeklistDto>
        {
            new WeeklistDto
            {
                Id = 1,
                Number = 568,
                OrderNumber = "EX123456",
                Supplier = "TVC",
                ShippingNumber = "Shipment123456"
            }
        };

        _weeklistServiceMock
            .Setup(s => s.GetAllWeeklistsAsync())
            .ReturnsAsync(mockWeeklists);

        var result = await _weeklistController.GetAllWeeklists();

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(200, okResult.StatusCode);
        Assert.Equal(mockWeeklists, okResult.Value);
    }

    [Fact]
    public async Task GetWeeklist_ReturnsOk_WithData()
    {
        var mockWeeklist = new WeeklistDto
        {
            Id = 1,
            Number = 568,
            OrderNumber = "EX123456",
            Supplier = "TVC",
            ShippingNumber = "Shipment123456"
        };

        _weeklistServiceMock
            .Setup(s => s.GetWeeklistAsync(mockWeeklist.Id))
            .ReturnsAsync(mockWeeklist);

        var result = await _weeklistController.GetWeeklist(mockWeeklist.Id);

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(200, okResult.StatusCode);
        Assert.Equal(mockWeeklist, okResult.Value);
    }

    [Fact]
    public async Task GetAllWeeklists_Returns500_OnException()
    {
        _weeklistServiceMock
            .Setup(s => s.GetAllWeeklistsAsync())
            .ThrowsAsync(new Exception("Database error"));

        var result = await _weeklistController.GetAllWeeklists();

        var objectResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(500, objectResult.StatusCode);
        Assert.Contains("Something went wrong", objectResult.Value!.ToString());
    }

    [Fact]
    public async Task GetWeeklist_Returns500_OnException()
    {
        _weeklistServiceMock
            .Setup(s => s.GetWeeklistAsync(It.IsAny<int>()))
            .ThrowsAsync(new Exception("Unexpected failure"));

        var result = await _weeklistController.GetWeeklist(1);

        var objectResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(500, objectResult.StatusCode);
        Assert.Contains("Something went wrong", objectResult.Value!.ToString());
    }

    private static FormFile CreateMockXlsFile(string filename, string content = "Mock Excel Content")
    {
        var stream = new MemoryStream(Encoding.UTF8.GetBytes(content));
        return new FormFile(stream, 0, stream.Length, "file", filename)
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
            Supplier = "TVC",
            ShippingNumber = "Shipment123456"
        };
    }
}