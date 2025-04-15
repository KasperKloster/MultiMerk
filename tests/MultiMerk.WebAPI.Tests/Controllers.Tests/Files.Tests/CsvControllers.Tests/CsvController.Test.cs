using Application.Files.Interfaces;
using Microsoft.AspNetCore.Http;
using Moq;
using System.Text;
using WebAPI.Controllers.Files.CsvControllers;
using Microsoft.AspNetCore.Mvc;
using Domain.Models.Files;

namespace MultiMerk.WebAPI.Tests.Controllers.Tests.Files.Tests.CsvControllers.Tests;

public class CsvControllerTest
{
    private readonly Mock<ICsvService> _fileServiceMock;
    private readonly CsvController _csvController;

    public CsvControllerTest()
    {
        _fileServiceMock = new Mock<ICsvService>();
        _csvController = new CsvController(_fileServiceMock.Object);
    }

    [Fact]
    public async Task Upload_ShouldReturnOk_WhenFileUploadIsSuccessful()
    {
        // Arrange
        var fileMock = CreateMockFormFile("col1,col2\nval1,val2");
        _fileServiceMock.Setup(s => s.UploadCsv(fileMock))
            .ReturnsAsync(FilesResult.SuccessResult());

        // Act
        var result = await _csvController.Upload(fileMock);

        // Assert
        Assert.IsType<OkResult>(result);
    }

    [Fact]
    public async Task Upload_ReturnsBadRequest_WhenUploadFails()
    {
        // Arrange
        var fileMock = CreateMockFormFile("invalid csv");
        _fileServiceMock.Setup(s => s.UploadCsv(fileMock))
            .ReturnsAsync(FilesResult.Fail("Upload failed"));

        // Act
        var result = await _csvController.Upload(fileMock);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Upload failed", badRequestResult.Value);
    }

    private IFormFile CreateMockFormFile(string content)
    {
        var stream = new MemoryStream(Encoding.UTF8.GetBytes(content));
        return new FormFile(stream, 0, stream.Length, "file", "test.csv");
    }
}
