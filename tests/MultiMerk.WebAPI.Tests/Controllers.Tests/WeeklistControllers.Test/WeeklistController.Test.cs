using System.Text;
using Application.Services.Interfaces.Products;
using Application.Services.Interfaces.Weeklists;
using Domain.Entities.Files;
using Domain.Entities.Weeklists.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using WebAPI.Controllers.WeeklistControllers;

namespace MultiMerk.WebAPI.Tests.Controllers.Tests.WeeklistControllers.Test
{
    public class WeeklistControllerTest
    {
        private readonly Mock<IWeeklistService> _weeklistServiceMock;
        private readonly Mock<IProductService> _productServiceMock;
        private readonly WeeklistController _weeklistController;

        public WeeklistControllerTest()
        {
            _weeklistServiceMock = new Mock<IWeeklistService>();
            _productServiceMock = new Mock<IProductService>();
            _weeklistController = new WeeklistController(_weeklistServiceMock.Object, _productServiceMock.Object);
        }

        [Fact]
        public async Task CreateWeeklist_ReturnsOk_WhenUploadIsSuccessful()
        {
            // Arrange
            var fileMock = CreateMockXlsFile("test.xls");
            var weeklistMock = CreateMockWeeklist();

            _weeklistServiceMock
                .Setup(s => s.CreateWeeklist(It.IsAny<IFormFile>(), It.IsAny<Weeklist>()))
                .ReturnsAsync(FilesResult.SuccessResult());

            // Act
            var result = await _weeklistController.CreateWeeklist(fileMock, weeklistMock.Number, weeklistMock.OrderNumber, weeklistMock.Supplier);

            // Assert
            var okResult = Assert.IsType<OkResult>(result);
            Assert.Equal(200, okResult.StatusCode);
        }

        [Fact]
        public async Task CreateWeeklist_ReturnsBadRequest_WhenUploadFails()
        {
            // Arrange
            var fileMock = CreateMockXlsFile("bad.xls");
            var weeklistMock = CreateMockWeeklist();

            _weeklistServiceMock
                .Setup(s => s.CreateWeeklist(It.IsAny<IFormFile>(), It.IsAny<Weeklist>()))
                .ReturnsAsync(FilesResult.Fail("Upload failed"));

            // Act
            var result = await _weeklistController.CreateWeeklist(fileMock, weeklistMock.Number, weeklistMock.OrderNumber, weeklistMock.Supplier);

            // Assert
            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, badRequest.StatusCode);
            Assert.Equal("Upload failed", badRequest.Value);
        }

        [Fact]
        public async Task AssignEan_ReturnsOk_WhenUpdateIsSuccessful()
        {
            // Arrange
            var fileMock = CreateMockXlsFile("ean.xls");

            _productServiceMock
                .Setup(s => s.UpdateProductsFromFile(It.IsAny<IFormFile>()))
                .ReturnsAsync(FilesResult.SuccessResult());

            // Act
            var result = await _weeklistController.AssignEan(fileMock);

            // Assert
            var okResult = Assert.IsType<OkResult>(result);
            Assert.Equal(200, okResult.StatusCode);
        }

        [Fact]
        public async Task AssignEan_ReturnsBadRequest_WhenUpdateFails()
        {
            // Arrange
            var fileMock = CreateMockXlsFile("ean_bad.xls");

            _productServiceMock
                .Setup(s => s.UpdateProductsFromFile(It.IsAny<IFormFile>()))
                .ReturnsAsync(FilesResult.Fail("EAN update failed"));

            // Act
            var result = await _weeklistController.AssignEan(fileMock);

            // Assert
            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, badRequest.StatusCode);
            Assert.Equal("EAN update failed", badRequest.Value);
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
                Supplier = "TVC"
            };
        }
    }
}