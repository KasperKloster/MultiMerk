using System.Text;
using Infrastructure.Files;
using Moq;
using Microsoft.AspNetCore.Http;
using NPOI.HSSF.UserModel;

namespace MultiMerk.Infrastructure.Tests.Files.Test;

public class FilesParserTest
{
    private readonly FileParser _fileParser = new();

    [Fact]
    public void GetProductsFromXls_ShouldParseExcelFileCorrectly()
    {
        // Arrange
        var file = CreateMockExcelFile();

        // var service = new YourServiceOrClassThatParsesExcel(); // Replace with your actual class

        // Act
        var result = _fileParser.GetProductsFromXls(file);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
        Assert.Contains(result, p => p.Sku == "ABC123");
        Assert.Contains(result, p => p.Sku == "XYZ789");
    }

    private IFormFile CreateMockExcelFile()
    {
        var workbook = new HSSFWorkbook();
        var sheet = workbook.CreateSheet("Sheet1");

        // Create header
        var headerRow = sheet.CreateRow(0);
        headerRow.CreateCell(0).SetCellValue("Sku");

        // Add data
        var row1 = sheet.CreateRow(1);
        row1.CreateCell(0).SetCellValue("ABC123");

        var row2 = sheet.CreateRow(2);
        row2.CreateCell(0).SetCellValue("XYZ789");

        var stream = new MemoryStream();
        workbook.Write(stream);
        stream.Position = 0;

        return new FormFile(stream, 0, stream.Length, "file", "test.xls")
        {
            Headers = new HeaderDictionary(),
            ContentType = "application/vnd.ms-excel"
        };
    }

}
