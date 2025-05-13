using Application.DTOs.Weeklists;
using Application.Files.Interfaces.csv;
using Application.Services.Interfaces.Tasks;
using Application.Services.Interfaces.Weeklists;
using Domain.Entities.Files;
using Domain.Entities.Products;
using Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers.WeeklistControllers.ContentControllers
{
    [Route("api/weeklist/content/")]
    [ApiController]
    public class ContentController : WeeklistBaseController
    {
        private readonly IContentService _contentService;
        private readonly IAICsvService _aiCsvService;
        // private readonly ICsvService _csvService;        

        public ContentController(IWeeklistService weeklistService, IWeeklistTaskLinkService weeklistTaskLinkService, IContentService contentService, IAICsvService aiCsvService) : base(weeklistService, weeklistTaskLinkService)
        {
            _contentService = contentService;
            _aiCsvService = aiCsvService;            
        }

        [HttpPost("get-products-ready-for-ai-content")]
        // [Authorize(Roles = $"{Roles.Admin}")]
        public async Task<IActionResult> GetProductsReadyForAIContent([FromForm] int weeklistId)
        {
            try
            {
                // Getting products
                List<Product> products = await _contentService.GetProductsReadyForAI(weeklistId);
                // Converts products to csv (byte array)
                var csvBytes = _aiCsvService.GenerateProductsReadyForAICSV(products);
                // Get weeklist to create filename
                WeeklistDto weeklist = await _weeklistService.GetWeeklistAsync(weeklistId);
                var fileName = $"{weeklist.Number}-Ready-For-AI.csv";
                // Mark Current task as done, set next to ready                
                var updateTaskResult = await UpdateTaskStatusAndAdvanceNext(weeklistId, WeeklistTaskNameEnum.GetAIContentList, WeeklistTaskNameEnum.UploadAIContent);
                return File(csvBytes, "text/csv", fileName);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Something went wrong. Please try again. {ex.Message}");
            }
        }

        [HttpPost("upload-ai-content")]
        // [Authorize(Roles = $"{Roles.Admin}")]
        public async Task<IActionResult> UploadAIContent([FromForm] IFormFile file)
        {
            try
            {
                FilesResult aiProducts = _aiCsvService.GetProductsFromAI(file);                
                if (!aiProducts.Success)
                {
                    return BadRequest(aiProducts.Message);
                }
                
                await _contentService.InsertAIProductContent(aiProducts.Products);

                // Mark Current task as done, set next to ready                
                // var updateTaskResult = await UpdateTaskStatus(weeklistId, WeeklistTaskNameEnum.UploadAIContent, WeeklistTaskStatusEnum.Done);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Something went wrong. Please try again. {ex.Message}");
            }
        }
    }
}
