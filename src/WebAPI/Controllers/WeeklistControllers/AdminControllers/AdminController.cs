using Application.Services.Interfaces.Files;
using Application.Services.Interfaces.Products;
using Domain.Constants;
using Domain.Entities.Files;
using Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers.WeeklistControllers.AdminControllers
{
    [Route("api/weeklist/admin/")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IProductService _productService;                
        private readonly IZipService _zipService;

        public AdminController(IProductService productService, IZipService zipService)
        {
            _productService = productService;
            _zipService = zipService;
        }

        [HttpPost("assign-ean")]
        [Authorize(Roles = $"{Roles.Admin}")]
        
        public async Task<IActionResult> AssignEan([FromForm] IFormFile file, [FromForm] int weeklistId)
        {
            
            FilesResult result = await _productService.UpdateProductsFromXlsUpdateStatus(
                file,
                weeklistId,
                WeeklistTaskNameEnum.AssignEAN,
                WeeklistTaskStatusEnum.Done);
            
            return result.Success ? Ok() : BadRequest(result.Message);            
        }

        [HttpPost("insert-out-of-stock")]
        [Authorize(Roles = $"{Roles.Admin}")]
        public async Task<IActionResult> UploadOutOfStock([FromForm] IFormFile file, [FromForm] int weeklistId)
        {
                    
            FilesResult result = await _productService.UpdateOutOfStockAndTaskAdvance(
                file,
                weeklistId,
                WeeklistTaskNameEnum.InsertOutOfStock,
                WeeklistTaskNameEnum.CreateChecklist);

            return result.Success ? Ok() : BadRequest(result.Message);            
        }

        [HttpPost("import-product-list")]
        [Authorize(Roles = $"{Roles.Admin}")]
        public async Task<IActionResult> ImportProductList([FromForm] int weeklistId)
        {
            try
            {
                byte[] zipBytes = await _zipService.GetZipAdminImportUpdateStatus(
                    weeklistId,
                    WeeklistTaskNameEnum.ImportProductList,
                    WeeklistTaskStatusEnum.Done);

                return File(zipBytes, "application/zip", $"Weeklist-{weeklistId}-Admin.zip");
            }
            catch (Exception ex)
            {
                return BadRequest($"Could not generate ZIP: {ex.Message}");
            }      
        }
    }
}
