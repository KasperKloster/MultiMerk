using Application.DTOs.Weeklists;
using Application.Repositories.Weeklists;

namespace Application.Services.Weeklists;

public class WeeklistService
{
    private readonly IWeeklistRepository _weeklistRepository;

    public WeeklistService(IWeeklistRepository weeklistRepository)
    {
        _weeklistRepository = weeklistRepository;
    }

    public async Task<List<WeeklistDto>> GetAllWeeklistsAsync()
    {
        try{
            var weeklists = await _weeklistRepository.GetAllWeeklists();

            List<WeeklistDto> weeklistsDtos = weeklists.Select(w => new WeeklistDto {
                Id = w.Id,
                Number = w.Number,
                OrderNumber = w.OrderNumber,
                Supplier = w.Supplier,
                WeeklistTasks = w.WeeklistTaskLinks.Select(link => new WeeklistTaskLinkDto{
                    WeeklistTaskId = link.WeeklistTaskId,
                    WeeklistTask = link.WeeklistTask != null ? new WeeklistTaskDto{
                        Id = link.WeeklistTask.Id,
                        Name = link.WeeklistTask.Name
                    } : null,
                    WeeklistTaskStatusId = link.WeeklistTaskStatusId,
                    Status = link.WeeklistTaskStatus != null ? new WeeklistTaskStatusDto
                    {
                        Id = link.WeeklistTaskStatus.Id,
                        Status = link.WeeklistTaskStatus.Status
                    } : null
                }).ToList()
            }).ToList();

            return weeklistsDtos;
        
        } catch (Exception ex)
        {
            throw new Exception("Could not fetch weeklists", ex);
        }

        
    }
}
