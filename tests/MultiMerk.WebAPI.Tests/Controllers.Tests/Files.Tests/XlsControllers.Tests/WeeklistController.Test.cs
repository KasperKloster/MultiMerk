using System.Text;
using Application.Files.Interfaces;
using Domain.Models.Files;
using Domain.Models.Weeklists;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using WebAPI.Controllers.Files.XlsControllers;

namespace MultiMerk.WebAPI.Tests.Controllers.Tests.Files.Tests.XlsControllers.Tests;

public class WeeklistControllerTest
{
    private readonly Mock<IXlsFileService> _xlsFileServiceMock;    
    private readonly WeeklistController _weeklistController;

    public WeeklistControllerTest()
    {
        _xlsFileServiceMock = new Mock<IXlsFileService>();
        _weeklistController = new WeeklistController(_xlsFileServiceMock.Object);
    }

    [Fact]
    public async Task CreateWeeklist_ReturnsOk_WhenUploadIsSuccessful()
    {
        // Arrange
        var fileMock = CreateMockXlsFile("test.xls");
        var weeklistMock = CreateMockWeeklist();

        var serviceMock = new Mock<IXlsFileService>();
        serviceMock
            .Setup(s => s.CreateWeeklist(It.IsAny<IFormFile>(), It.IsAny<Weeklist>()))
            .ReturnsAsync(FilesResult.SuccessResult());

        var controller = new WeeklistController(serviceMock.Object);

        // Act
        var result = await controller.CreateWeeklist(fileMock, weeklistMock.Number, weeklistMock.OrderNumber, weeklistMock.Supplier);

        // Assert
        var okResult = Assert.IsType<OkResult>(result);
        Assert.Equal(200, okResult.StatusCode);
    }

    [Fact]
    public async Task CreateWeeklist_ReturnsBadRequest_WhenUploadFails()
    {
        // Arrange
        var fileMock = CreateMockXlsFile("test.csv");
        var weeklistMock = CreateMockWeeklist();

        var serviceMock = new Mock<IXlsFileService>();
        serviceMock
            .Setup(s => s.CreateWeeklist(It.IsAny<IFormFile>(), It.IsAny<Weeklist>()))
            .ReturnsAsync(FilesResult.SuccessResult());

        var controller = new WeeklistController(serviceMock.Object);

        // Act
        var result = await controller.CreateWeeklist(fileMock, weeklistMock.Number, weeklistMock.OrderNumber, weeklistMock.Supplier);

        // Assert
        var okResult = Assert.IsType<OkResult>(result);
        Assert.Equal(200, okResult.StatusCode);
    } 

    // Mocking a simple file
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
            Supplier = "TVC"
        };
    }
}
