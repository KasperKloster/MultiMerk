using System.Text;
using Application.Files.Interfaces;
using Application.Repositories;
using Application.Repositories.Weeklists;
using Application.Services.Weeklists;
using Domain.Entities.Files;
using Domain.Entities.Weeklists.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Identity;
using Moq;
using Xunit;
using Application.Repositories.ApplicationUsers;
using Domain.Entities.Authentication;
using System.Security.Claims;

namespace MultiMerk.Application.Tests.Services.Tests.WeeklistTests;

public class WeeklistServiceTests
{
    private readonly Mock<IWeeklistRepository> _weeklistRepo = new();
    private readonly Mock<IXlsFileService> _xlsFileService = new();
    private readonly Mock<IProductRepository> _productRepo = new();
    private readonly Mock<IWeeklistTaskRepository> _taskRepo = new();
    private readonly Mock<IWeeklistTaskLinkRepository> _taskLinkRepo = new();
    private readonly Mock<IWeeklistUserRoleAssignmentRepository> _roleAssignmentRepo = new();
    private readonly Mock<IApplicationUserRepository> _userRepo = new();
    private readonly Mock<UserManager<ApplicationUser>> _userManager;

    public WeeklistServiceTests()
    {
        var userStoreMock = new Mock<IUserStore<ApplicationUser>>();
        _userManager = new Mock<UserManager<ApplicationUser>>(
            userStoreMock.Object,
            null, null, null, null, null, null, null, null);
    }

    private WeeklistService CreateService() => new(
        _weeklistRepo.Object,
        _xlsFileService.Object,
        _productRepo.Object,
        _taskRepo.Object,
        _taskLinkRepo.Object,
        _roleAssignmentRepo.Object,
        _userRepo.Object,
        _userManager.Object
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
        _xlsFileService.Setup(x => x.GetProductsFromXls(It.IsAny<IFormFile>())).Returns(FilesResult.Fail("Invalid file extension."));

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
        var file = CreateMockFile("test.xls");
        var weeklist = CreateWeeklist();
        _xlsFileService.Setup(x => x.GetProductsFromXls(It.IsAny<IFormFile>())).Returns(FilesResult.Fail("No products found in the file."));

        var service = CreateService();

        // Act
        var result = await service.CreateWeeklist(file, weeklist);

        // Assert
        Assert.False(result.Success);
        Assert.Equal("No products found in the file.", result.Message);
        _weeklistRepo.Verify(r => r.AddAsync(It.IsAny<Weeklist>()), Times.Never);
    }


}