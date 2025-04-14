using System.Text;
using Application.Files;
using Application.Files.Interfaces;
using Microsoft.AspNetCore.Http;
using Moq;
using Xunit;

namespace MultiMerk.Application.Tests.Files.Tests;


public class FileServiceTest
{
    private readonly Mock<IFileParser> _fileParserMock;
    private readonly FileService _fileService;
    public FileServiceTest()
    {
        _fileParserMock = new Mock<IFileParser>();
        _fileService = new FileService(_fileParserMock.Object);
    }


    [Theory]
    [InlineData("file.csv", true)]
    [InlineData("file.xls", true)]
    [InlineData("file.txt", false)]
    [InlineData("file.exe", false)]
    public void IsValidFileExtension_ReturnsExpectedResult(string fileName, bool expectedResult)
    {
        // Arrange
        var fileMock = new Mock<IFormFile>();
        fileMock.Setup(f => f.FileName).Returns(fileName);
        // Act
        var result = _fileService.IsValidFileExtension(fileMock.Object);
        // Assert
        Assert.Equal(expectedResult, result);
    }

    [Fact]
    public void GetDelimiterFromCsv_DelegatesToFileParser()
    {
        // Arrange
        var fileMock = new Mock<IFormFile>();
        _fileParserMock.Setup(fp => fp.GetDelimiterFromCsv(It.IsAny<IFormFile>())).Returns(',');

        // Act
        var result = _fileService.GetDelimiterFromCsv(fileMock.Object);

        // Assert
        Assert.Equal(',', result);
        _fileParserMock.Verify(fp => fp.GetDelimiterFromCsv(fileMock.Object), Times.Once);
    }

    [Fact]
    public void GetBadDelimiterFromCsv_DelegatesToFileParser()
    {
        // Arrange
        var fileMock = new Mock<IFormFile>();
        _fileParserMock.Setup(fp => fp.GetDelimiterFromCsv(It.IsAny<IFormFile>())).Returns(',');

        // Act
        var result = _fileService.GetDelimiterFromCsv(fileMock.Object);

        // Assert
        Assert.Equal(',', result);
        _fileParserMock.Verify(fp => fp.GetDelimiterFromCsv(fileMock.Object), Times.Once);
    }


}

