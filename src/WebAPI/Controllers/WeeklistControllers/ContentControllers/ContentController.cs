using Application.Services.Interfaces.Tasks;
using Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers.WeeklistControllers.ContentControllers
{
    [Route("api/weeklist/content/")]
    [ApiController]
    public class ContentController : WeeklistBaseController
    {
        public ContentController(IWeeklistTaskLinkService weeklistTaskLinkService) : base(weeklistTaskLinkService)
        {
        }

        [HttpPost("create-ai-content")]
        // [Authorize(Roles = $"{Roles.Admin}")]
        public async Task<IActionResult> CreateAIContent([FromForm] int weeklistId)
        {
            try
            {
                // Mark Current task as done, set next to ready                
                var updateTaskResult = await UpdateTaskStatusAndAdvanceNext(weeklistId, WeeklistTaskName.CreateAIcontentList);                
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Something went wrong. Please try again. {ex.Message}");
            }
            return Ok();
        }

        [HttpPost("upload-ai-content")]
        // [Authorize(Roles = $"{Roles.Admin}")]
        public async Task<IActionResult> UploadAIContent([FromForm] int weeklistId)
        {
            try
            {
                // Mark Current task as done, set next to ready                
                var updateTaskResult = await UpdateTaskStatusAndAdvanceNext(weeklistId, WeeklistTaskName.UploadAIContent);                
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Something went wrong. Please try again. {ex.Message}");
            }
            return Ok();
        }


    }
}
