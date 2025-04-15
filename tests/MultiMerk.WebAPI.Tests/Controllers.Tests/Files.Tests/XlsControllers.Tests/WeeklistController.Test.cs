using System.Text;
using Application.Files.Interfaces;
using Domain.Models.Files;
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
    public async Task Upload_ReturnOk_WhenWeeklistIsUploadIsSuccessful()
    {
        // Arrange
        var fileMock = CreateMockXlsFile("ValidFileExtension.xls");
        _xlsFileServiceMock.Setup(s => s.CreateWeeklist(fileMock)).ReturnsAsync(FilesResult.SuccessResult());

        // Act
        var result = await _weeklistController.CreateWeeklist(fileMock);

        // Assert
        Assert.IsType<OkResult>(result);        
    }

    [Fact]
    public async Task Upload_ReturnsBadRequest_WhenUploadFails()
    {
        // Arrange
        var fileMock = CreateMockXlsFile("NonValidFileExtension.csv");
        _xlsFileServiceMock.Setup(s => s.CreateWeeklist(fileMock)).ReturnsAsync(FilesResult.Fail(message: "Invalid file extension"));

        // Act
        var result = await _weeklistController.CreateWeeklist(fileMock);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Invalid file extension", badRequestResult.Value);  
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

}
