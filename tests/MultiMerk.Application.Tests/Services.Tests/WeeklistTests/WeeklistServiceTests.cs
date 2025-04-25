using Application.Repositories.Weeklists;
using Application.Services.Weeklists;
using Domain.Entities.Weeklists.Entities;
using Domain.Entities.Weeklists.WeeklistTaskLinks;
using Domain.Entities.Weeklists.WeeklistTasks;
using Moq;

namespace MultiMerk.Application.Tests.Services.Tests.WeeklistTests;
public class WeeklistServiceTests
{
    [Fact]
    public async Task GetAllWeeklistsAsync_ReturnsMappedWeeklistDtos()
    {
        // Arrange
        var mockRepo = new Mock<IWeeklistRepository>();

        var sampleWeeklists = new List<Weeklist>
        {
            new Weeklist
            {
                Id = 1,
                Number = 10,
                OrderNumber = "ORD001",
                Supplier = "Test Supplier",
                WeeklistTaskLinks = new List<WeeklistTaskLink>
                {
                    new WeeklistTaskLink
                    {
                        WeeklistTaskId = 101,
                        WeeklistTask = new WeeklistTask { Id = 101, Name = "Test Task" },
                        WeeklistTaskStatusId = 201,
                        WeeklistTaskStatus = new WeeklistTaskStatus { Id = 201, Status = "Done" }
                    }
                }
            }
        };

        mockRepo.Setup(repo => repo.GetAllWeeklists())
            .ReturnsAsync(sampleWeeklists);

        var service = new WeeklistService(mockRepo.Object);

        // Act
        var result = await service.GetAllWeeklistsAsync();

        // Assert
        Assert.Single(result);
        Assert.Equal(1, result[0].Id);
        Assert.Equal("ORD001", result[0].OrderNumber);
        Assert.Single(result[0].WeeklistTasks);
        Assert.Equal("Test Task", result[0].WeeklistTasks[0].WeeklistTask?.Name);
        Assert.Equal("Done", result[0].WeeklistTasks[0].Status?.Status);
    }
}