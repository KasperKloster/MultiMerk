using System.Text;
using Application.Files.Interfaces;
using Application.Files.Services;
using Application.Repositories;
using Application.Repositories.Weeklists;
using Domain.Models.Products;
using Domain.Models.Weeklists;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Moq;

namespace MultiMerk.Application.Tests.Files.Tests.Services.Test;

public class XlsFileServiceTests
{
    private readonly Mock<IFileParser> _mockParser;
    private readonly Mock<IProductRepository> _mockProductRepo;
    private readonly Mock<IWeeklistRepository> _mockWeeklistRepo;
    private readonly Mock<IWeeklistTaskRepository> _mockWeeklistTaskRepository;
    private readonly Mock<IWeeklistTaskLinkRepository> _mockWeeklistTaskLinkRepository;

    public XlsFileServiceTests(Mock<IFileParser> mockParser, Mock<IProductRepository> mockProductRepo, Mock<IWeeklistRepository> mockWeeklistRepo, Mock<IWeeklistTaskRepository> mockWeeklistTaskRepository, Mock<IWeeklistTaskLinkRepository> mockWeeklistTaskLinkRepository)
    {
        _mockParser = mockParser;
        _mockProductRepo = mockProductRepo;
        _mockWeeklistRepo = mockWeeklistRepo;
        _mockWeeklistTaskRepository = mockWeeklistTaskRepository;
        _mockWeeklistTaskLinkRepository = mockWeeklistTaskLinkRepository;
    }

    [Fact]
    public async Task CreateWeeklist_ShouldFail_WhenFileExtensionIsInvalid()
    {
        // Arrange
        Weeklist weeklist = CreateMockWeeklist();
        var service = new XlsFileService(_mockParser.Object, _mockProductRepo.Object, _mockWeeklistRepo.Object, _mockWeeklistTaskRepository.Object, _mockWeeklistTaskLinkRepository.Object);
        var invalidFile = CreateMockFile("test.csv");

        // Act
        var result = await service.CreateWeeklist(invalidFile, weeklist);

        // Assert
        Assert.False(result.Success);
        Assert.Equal("Invalid file extension.", result.Message);
        _mockProductRepo.Verify(r => r.AddRangeAsync(It.IsAny<List<Product>>()), Times.Never);
        _mockWeeklistRepo.Verify(r => r.AddAsync(It.IsAny<Weeklist>()), Times.Never);
    }

    [Fact]
    public async Task CreateWeeklist_ShouldFail_WhenNoProductsFound()
    {
        // Arrange
        Weeklist weeklist = CreateMockWeeklist();
        _mockParser.Setup(p => p.GetProductsFromXls(It.IsAny<IFormFile>()))
                   .Returns(new List<Product>());

        var service = new XlsFileService(_mockParser.Object, _mockProductRepo.Object, _mockWeeklistRepo.Object, _mockWeeklistTaskRepository.Object, _mockWeeklistTaskLinkRepository.Object);

        var file = CreateMockFile("test.xls");

        // Act
        var result = await service.CreateWeeklist(file, weeklist);

        // Assert
        Assert.False(result.Success);
        Assert.Equal("No products found in the file.", result.Message);
        _mockProductRepo.Verify(r => r.AddRangeAsync(It.IsAny<List<Product>>()), Times.Never);
        _mockWeeklistRepo.Verify(r => r.AddAsync(It.IsAny<Weeklist>()), Times.Never);
    }

    [Fact]
    public async Task CreateWeeklist_ShouldSucceed_WhenValidProductsAreParsed()
    {
        // Arrange
        Weeklist weeklist = CreateMockWeeklist(); // Id = 1
        var products = new List<Product> { new("123"), new("456") };

        _mockParser.Setup(p => p.GetProductsFromXls(It.IsAny<IFormFile>()))
                .Returns(products);

        // Simulate EF Core setting Weeklist.Id after AddAsync
        _mockWeeklistRepo.Setup(r => r.AddAsync(It.IsAny<Weeklist>()))
                        .Callback<Weeklist>(w => w.Id = 42) // Simulate EF Core setting the ID
                        .Returns(Task.CompletedTask)
                        .Verifiable();

        _mockProductRepo.Setup(r => r.AddRangeAsync(It.IsAny<List<Product>>()))
                        .Callback<IEnumerable<Product>>(prods =>
                        {
                            // Assert inside callback that each product has correct WeeklistId set
                            foreach (var prod in prods)
                            {
                                Assert.Equal(42, prod.WeeklistId);
                            }
                        })
                        .Returns(Task.CompletedTask)
                        .Verifiable();
        
        var service = new XlsFileService(_mockParser.Object, _mockProductRepo.Object, _mockWeeklistRepo.Object, _mockWeeklistTaskRepository.Object, _mockWeeklistTaskLinkRepository.Object);    
        var file = CreateMockFile("test.xls");

        // Act
        var result = await service.CreateWeeklist(file, weeklist);

        // Assert
        Assert.True(result.Success);
        Assert.Null(result.Message);
        _mockWeeklistRepo.Verify(r => r.AddAsync(weeklist), Times.Once);
        _mockProductRepo.Verify(r => r.AddRangeAsync(It.IsAny<List<Product>>()), Times.Once);
    }


    private IFormFile CreateMockFile(string name = "test.xls")
    {
        var content = "dummy content";
        var stream = new MemoryStream(Encoding.UTF8.GetBytes(content));
        return new FormFile(stream, 0, stream.Length, "file", name)
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
