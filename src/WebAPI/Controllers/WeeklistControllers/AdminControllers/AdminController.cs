using System.Diagnostics;
using Application.Services.Interfaces.Products;
using Application.Services.Interfaces.Tasks;
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

        public AdminController(IProductService productService, IWeeklistTaskLinkService weeklistTaskLinkService) : base(weeklistTaskLinkService)
        {
            _productService = productService;            
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
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Something went wrong. Please try again. {ex.Message}");
            }
            return Ok();
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

        [HttpPost("create-final-list")]
        // [Authorize(Roles = $"{Roles.Admin}")]
        public async Task<IActionResult> CreateFinalList([FromForm] int weeklistId)
        {
            try
            {
                // Mark Current task as done, set next to ready                          
                var updateTaskResult = await UpdateTaskStatusAndAdvanceNext(weeklistId, WeeklistTaskName.CreateFinalList, WeeklistTaskName.ImportProductList);

            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Something went wrong. Please try again. {ex.Message}");
            }
            return Ok();
        }

        [HttpPost("import-product-list")]
        // [Authorize(Roles = $"{Roles.Admin}")]
        public async Task<IActionResult> ImportProductList([FromForm] int weeklistId)
        {
            try
            {

                // Mark Current task as done, set next to ready                
                var updateTaskResult = await UpdateTaskStatusAndAdvanceNext(weeklistId, WeeklistTaskName.ImportProductList, WeeklistTaskName.CreateTranslations);                
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Something went wrong. Please try again. {ex.Message}");
            }
            return Ok();
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
                
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Something went wrong. Please try again. {ex.Message}");
            }

            return Ok();
        }

    }
}
