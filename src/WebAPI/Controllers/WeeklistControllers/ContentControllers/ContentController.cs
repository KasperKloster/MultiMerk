using Application.DTOs.Weeklists;
using Application.Files.Interfaces;
using Application.Services.Interfaces.Tasks;
using Application.Services.Interfaces.Weeklists;
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
        private readonly ICsvService _csvService;
        private readonly IWeeklistService _weeklistService;

        public ContentController(IWeeklistTaskLinkService weeklistTaskLinkService, IContentService contentService, ICsvService csvService, IWeeklistService weeklistService) : base(weeklistTaskLinkService)
        {
            _contentService = contentService;
            _csvService = csvService;
            _weeklistService = weeklistService;
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
                var csvBytes = _csvService.GenerateProductsReadyForAICSV(products);
                // Get weeklist to create filename
                WeeklistDto weeklist = await _weeklistService.GetWeeklistAsync(weeklistId);
                var fileName = $"{weeklist.Number}-Ready-For-AI.csv";
                // Mark Current task as done, set next to ready                
                var updateTaskResult = await UpdateTaskStatusAndAdvanceNext(weeklistId, WeeklistTaskName.GetAIContentList);
                return File(csvBytes, "text/csv", fileName);
            }

            catch (Exception ex)
            {
                return StatusCode(500, $"Something went wrong. Please try again. {ex.Message}");
            }
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
