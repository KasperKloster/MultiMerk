using Application.Services.Interfaces.Tasks;
using Application.Services.Interfaces.Weeklists;
using Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers.WeeklistControllers;

public abstract class WeeklistBaseController : ControllerBase
{
    protected readonly IWeeklistService _weeklistService;
    protected readonly IWeeklistTaskLinkService _weeklistTaskLinkService;

    protected WeeklistBaseController(IWeeklistService weeklistService, IWeeklistTaskLinkService weeklistTaskLinkService)
    {
        _weeklistService = weeklistService;
        _weeklistTaskLinkService = weeklistTaskLinkService;
    }

    protected async Task<IActionResult> UpdateTaskStatus(int weeklistId, WeeklistTaskNameEnum taskName, WeeklistTaskStatusEnum newTaskStatus)
    {
        try
        {
            var result = await _weeklistTaskLinkService.UpdateTaskStatus(
                weeklistId: weeklistId,
                currentTask: taskName,
                newTaskStatus: newTaskStatus);
            if (!result.Success) 
            {
                return BadRequest(result.Message);
            }
            return Ok();
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Something went wrong. Please try again. {ex.Message}");
        }
    }

    protected async Task<IActionResult> UpdateTaskStatusAndAdvanceNext(int weeklistId, WeeklistTaskNameEnum currentTask, WeeklistTaskNameEnum newTask)
    {
        try
        {
            var result = await _weeklistTaskLinkService.UpdateTaskStatusAndAdvanceNext(
                weeklistId: weeklistId,
                currentTask: currentTask,
                newTask: newTask);
            if (!result.Success) 
            {
                return BadRequest(result.Message);
            }
            return Ok();
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Something went wrong. Please try again. {ex.Message}");
        }
    }
}
