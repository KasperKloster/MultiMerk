using Application.Services.Interfaces.Weeklists;
using Domain.Constants;
using Domain.Entities.Files;
using Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers.WeeklistControllers.ContentControllers
{
    [Route("api/weeklist/content/")]
    [ApiController]
    public class ContentController : ControllerBase
    {
        private readonly IContentService _contentService;             

        public ContentController(IContentService contentService)
        {
            _contentService = contentService;            
        }

        [HttpPost("get-products-ready-for-ai-content")]        
        [Authorize(Roles = $"{Roles.Admin},{Roles.Writer}")]
        public async Task<IActionResult> GetProductsReadyForAIContent([FromForm] int weeklistId)
        {
            try
            {
                FilesResult result = await _contentService.GetAIProductsAndTaskAdvance(
                    weeklistId,
                    WeeklistTaskNameEnum.GetAIContentList,
                    WeeklistTaskNameEnum.UploadAIContent);

                if (!result.Success){
                    return BadRequest();
                }

                return File(result.FileBytes, "text/csv", result.FileName);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("upload-ai-content")]
        [Authorize(Roles = $"{Roles.Admin},{Roles.Writer}")]
        public async Task<IActionResult> UploadAIContent([FromForm] IFormFile file, [FromForm] int weeklistId)
        {
            try
            {
                FilesResult result = await _contentService.InsertAIProductsUpdateStatus(
                    file,
                    weeklistId,
                    WeeklistTaskNameEnum.UploadAIContent,
                    WeeklistTaskStatusEnum.Done);
                if (!result.Success){
                    return BadRequest();
                }
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
