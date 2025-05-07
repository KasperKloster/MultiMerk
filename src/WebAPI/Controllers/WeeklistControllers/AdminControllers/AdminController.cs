using System.IO.Compression;
using Application.DTOs.Weeklists;
using Application.Files.Interfaces;
using Application.Services.Interfaces.Products;
using Application.Services.Interfaces.Tasks;
using Application.Services.Interfaces.Weeklists;
using Domain.Entities.Files;
using Domain.Entities.Products;
using Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers.WeeklistControllers.AdminControllers
{
    [Route("api/weeklist/admin/")]
    [ApiController]
    public class AdminController : WeeklistBaseController
    {
        private readonly IProductService _productService;
        private readonly ICsvService _csvService;
        private readonly IWeeklistService _weeklistService;
        private readonly IZipService _zipService;

        public AdminController(IProductService productService, IWeeklistTaskLinkService weeklistTaskLinkService, ICsvService csvService, IWeeklistService weeklistService, IZipService zipService) : base(weeklistTaskLinkService)
        {
            _productService = productService;
            _csvService = csvService;
            _weeklistService = weeklistService;
            _zipService = zipService;
        }

        [HttpPost("assign-ean")]
        // [Authorize(Roles = $"{Roles.Admin}")]
        public async Task<IActionResult> AssignEan([FromForm] IFormFile file, [FromForm] int weeklistId)
        {
            try
            {
                // Send to product service
                var result = await _productService.UpdateProductsFromFile(file);
                if (!result.Success)
                {
                    return BadRequest(result.Message);
                }

                // Mark Current task as done, set next to ready                
                var updateTaskResult = await UpdateTaskStatus(weeklistId, WeeklistTaskName.AssignEAN, WeeklistTaskStatus.Done);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Something went wrong. Please try again. {ex.Message}");
            }
        }

        [HttpPost("insert-out-of-stock")]
        // [Authorize(Roles = $"{Roles.Admin}")]
        public async Task<IActionResult> UploadOutOfStock([FromForm] IFormFile file, [FromForm] int weeklistId)
        {
            try
            {
                // Get products from list
                FilesResult result = _productService.GetProductsFromOutOfStock(file);
                if (result.Success && result.StockProducts is not null && result.StockProducts.Count > 0)
                {
                    Dictionary<string, int> stockProducts = result.StockProducts;
                    FilesResult updateResult = await _productService.UpdateProductsFromStockProducts(stockProducts);
                    
                    if(updateResult.Success){
                        // Mark Current task as done, set next to ready                
                        var updateTaskResult = await UpdateTaskStatusAndAdvanceNext(weeklistId: weeklistId, currentTask: WeeklistTaskName.InsertOutOfStock, newTask: WeeklistTaskName.CreateChecklist);
                    }
                }
                
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Something went wrong. Please try again. {ex.Message}");
            }
        }

        [HttpPost("import-product-list")]
        // [Authorize(Roles = $"{Roles.Admin}")]
        public async Task<IActionResult> ImportProductList([FromForm] int weeklistId)
        {
            try            
            {
                WeeklistDto weeklist = await _weeklistService.GetWeeklistAsync(weeklistId);
                List<Product> products = await _productService.GetProductsFromWeeklist(weeklistId);
                byte[] zipBytes = await _zipService.CreateZipAdminImportAsync(weeklist, products);
                // Mark Current task as done, set next to ready                
                await UpdateTaskStatusAndAdvanceNext(weeklistId, WeeklistTaskName.ImportProductList, WeeklistTaskName.CreateTranslations);
                return File(zipBytes, "application/zip", $"{weeklist.Number}-Admin.zip");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Something went wrong. Please try again. {ex.Message}");
            }            
        }

        [HttpPost("create-translations")]
        // [Authorize(Roles = $"{Roles.Admin}")]
        public async Task<IActionResult> CreateTranslations([FromForm] int weeklistId)
        {
            try
            {

                // Mark Current task as done
                var updateResult = await _weeklistTaskLinkService.UpdateTaskStatus(
                    weeklistId: weeklistId,
                    currentTask: WeeklistTaskName.CreateTranslations,
                    newTaskStatus: WeeklistTaskStatus.Done);

                if (!updateResult.Success)
                {
                    return BadRequest(updateResult.Message);
                }
                return Ok();
                
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Something went wrong. Please try again. {ex.Message}");
            }

        }

    }
}
