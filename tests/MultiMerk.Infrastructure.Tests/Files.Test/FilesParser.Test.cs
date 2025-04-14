using System.Text;
using Infrastructure.Files;
using Xunit;
using Moq;
using Microsoft.AspNetCore.Http;

namespace MultiMerk.Infrastructure.Tests.Files.Test;

public class FilesParserTest
{
    private readonly FileParser _fileParser = new();

    [Theory]
    [InlineData("col1|col2|col3")] // Pipe is not a supported delimiter
    [InlineData("col1:col2:col3")] // Colon is not supported
    [InlineData("col1col2col3")]   // No delimiter at all
    public void GetDelimiterFromCsv_ThrowsException_WhenDelimiterIsInvalid(string csvLine)
    {
        // Arrange
        var content = Encoding.UTF8.GetBytes(csvLine);
        var stream = new MemoryStream(content);
        var fileMock = new Mock<IFormFile>();
        fileMock.Setup(f => f.OpenReadStream()).Returns(stream);

        // Act & Assert
        var exception = Assert.Throws<InvalidDataException>(() =>
            _fileParser.GetDelimiterFromCsv(fileMock.Object));

        Assert.Equal("Unable to determine delimiter.", exception.Message);
    }
}
