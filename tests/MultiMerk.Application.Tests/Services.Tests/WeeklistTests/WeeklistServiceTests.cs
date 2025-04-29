using System.Text;
using Application.Files.Interfaces;
using Application.Repositories;
using Application.Repositories.Weeklists;
using Application.Services.Weeklists;
using Domain.Entities.Files;
using Domain.Entities.Products;
using Domain.Entities.Weeklists.Entities;
using Domain.Entities.Weeklists.WeeklistTaskLinks;
using Domain.Entities.Weeklists.WeeklistTasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Moq;

namespace MultiMerk.Application.Tests.Services.Tests.WeeklistTests;
public class WeeklistServiceTests
{
    private readonly Mock<IWeeklistRepository> _weeklistRepo = new();
    private readonly Mock<IXlsFileService> _xlsFileService = new();
    private readonly Mock<IProductRepository> _productRepo = new();
    private readonly Mock<IWeeklistTaskRepository> _taskRepo = new();
    private readonly Mock<IWeeklistTaskLinkRepository> _taskLinkRepo = new();

    private WeeklistService CreateService() => new(
        _weeklistRepo.Object,
        _xlsFileService.Object,
        _productRepo.Object,
        _taskRepo.Object,
        _taskLinkRepo.Object
    );

    private static IFormFile CreateMockFile(string name = "test.xls")
    {
        var stream = new MemoryStream(Encoding.UTF8.GetBytes("dummy"));
        return new FormFile(stream, 0, stream.Length, "file", name);
    }

    private static Weeklist CreateWeeklist() => new()
    {
        Id = 99,
        Number = 202,
        OrderNumber = "ORD-999",
        Supplier = "SupplierX"
    };

    [Fact]
    public async Task CreateWeeklist_ShouldFail_WhenFileInvalid()
    {
        // Arrange
        var file = CreateMockFile("invalid.csv");
        var weeklist = CreateWeeklist();
        _xlsFileService.Setup(x => x.GetProductsFromXls(file))
            .ReturnsAsync(new FilesResult
            {
                Success = false,
                Message = "Invalid file extension.",
                Products = new List<Product>() // Prevent null reference
            });

        var service = CreateService();

        // Act
        var result = await service.CreateWeeklist(file, weeklist);

        // Assert
        Assert.False(result.Success);
        Assert.Equal("Invalid file extension.", result.Message);
        _weeklistRepo.Verify(r => r.AddAsync(It.IsAny<Weeklist>()), Times.Never);
    }

    [Fact]
    public async Task CreateWeeklist_ShouldFail_WhenNoProductsFound()
    {
        // Arrange
        var file = CreateMockFile();
        var weeklist = CreateWeeklist();
        _xlsFileService.Setup(x => x.GetProductsFromXls(file))
            .ReturnsAsync(new FilesResult
            {
                Success = false,
                Message = "No products found in the file.",
                Products = new List<Product>() // Prevent null reference
            });

        var service = CreateService();

        // Act
        var result = await service.CreateWeeklist(file, weeklist);

        // Assert
        Assert.False(result.Success);
        Assert.Equal("No products found in the file.", result.Message);
        _weeklistRepo.Verify(r => r.AddAsync(It.IsAny<Weeklist>()), Times.Never);
    }

    [Fact]
    public async Task CreateWeeklist_ShouldFail_WhenWeeklistSaveFails()
    {
        var file = CreateMockFile();
        var weeklist = CreateWeeklist();

        var products = new List<Product> { new("p1"), new("p2") };

        _xlsFileService.Setup(x => x.GetProductsFromXls(file))
            .ReturnsAsync(FilesResult.SuccessResultWithProducts(products));

        _weeklistRepo.Setup(r => r.AddAsync(weeklist))
            .ThrowsAsync(new Exception("DB error"));

        var service = CreateService();

        var result = await service.CreateWeeklist(file, weeklist);

        Assert.False(result.Success);
        Assert.Contains("saving weeklist to database", result.Message);
        _productRepo.Verify(r => r.AddRangeAsync(It.IsAny<List<Product>>()), Times.Never);
    }

    [Fact]
    public async Task CreateWeeklist_ShouldSucceed_WhenEverythingValid()
    {
        var file = CreateMockFile();
        var weeklist = CreateWeeklist();
        var products = new List<Product> { new("p1"), new("p2") };

        _xlsFileService.Setup(x => x.GetProductsFromXls(file))
            .ReturnsAsync(FilesResult.SuccessResultWithProducts(products));

        _weeklistRepo.Setup(r => r.AddAsync(weeklist)).Returns(Task.CompletedTask);

        _productRepo.Setup(r => r.AddRangeAsync(It.IsAny<List<Product>>()))
            .Returns(Task.CompletedTask)
            .Verifiable();

        var tasks = new List<WeeklistTask>
        {
            new WeeklistTask { Id = 1, Name = "Give EAN" },
            new WeeklistTask { Id = 2, Name = "Check Stock" }
        };

        _taskRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(tasks);
        _taskLinkRepo.Setup(r => r.AddWeeklistTaskLinksAsync(It.IsAny<List<WeeklistTaskLink>>()))
            .Returns(Task.CompletedTask);

        var service = CreateService();

        var result = await service.CreateWeeklist(file, weeklist);

        Assert.True(result.Success);
        Assert.Null(result.Message);
        _productRepo.Verify();
    }
}