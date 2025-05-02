using System;
using Application.Services.Interfaces.Tasks;
using Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers.WeeklistControllers;

public class WeeklistBaseController : ControllerBase
{
    protected readonly IWeeklistTaskLinkService _weeklistTaskLinkService;
    
    protected WeeklistBaseController(IWeeklistTaskLinkService weeklistTaskLinkService)
    {
        _weeklistTaskLinkService = weeklistTaskLinkService;
    }

    protected async Task<IActionResult> UpdateTaskStatusAndAdvanceNext(int weeklistId, WeeklistTaskName taskName)
    {
        try
        {
            var result = await _weeklistTaskLinkService.UpdateTaskStatusAndAdvanceNext(
                weeklistId: weeklistId,
                currentTask: taskName,
                taskStatus: WeeklistTaskStatus.Done);

            if (!result.Success) {
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
