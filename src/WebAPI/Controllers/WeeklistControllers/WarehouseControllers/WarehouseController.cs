using Application.Services.Interfaces.Tasks;
using Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers.WeeklistControllers.WarehouseControllers
{
    [Route("api/weeklist/warehouse/")]
    [ApiController]
    public class WarehouseController : WeeklistBaseController
    {
        
        public WarehouseController(IWeeklistTaskLinkService weeklistTaskLinkService) : base(weeklistTaskLinkService)
        {            
            
        }

        [HttpPost("get-checklist")]
        // [Authorize(Roles = $"{Roles.Admin}")]
        public async Task<IActionResult> GetChecklist([FromForm] int weeklistId)
        {
            try
            {
                // Mark Current task as done, set next to ready
                var updateTaskResult = await UpdateTaskStatusAndAdvanceNext(weeklistId, WeeklistTaskName.AssignCorrectQuantity);                
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Something went wrong. Please try again. {ex.Message}");
            }
            return Ok();
        }   

    }
}
