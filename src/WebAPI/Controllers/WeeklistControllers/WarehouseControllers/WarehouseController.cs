using Application.DTOs.Weeklists;
using Application.Files.Interfaces;
using Application.Services.Interfaces.Products;
using Application.Services.Interfaces.Tasks;
using Application.Services.Interfaces.Weeklists;
using Domain.Entities.Products;
using Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers.WeeklistControllers.WarehouseControllers
{
    [Route("api/weeklist/warehouse/")]
    [ApiController]
    public class WarehouseController : WeeklistBaseController
    {
        
        private readonly IProductService _productService;
        private readonly IXlsFileService _xlsFileService;        

        public WarehouseController(IWeeklistService weeklistService, IWeeklistTaskLinkService weeklistTaskLinkService, IProductService productService, IXlsFileService xlsFileService) : base(weeklistService, weeklistTaskLinkService)
        {
            _productService = productService;            
            _xlsFileService = xlsFileService;            
        }

        [HttpPost("get-checklist")]
        public async Task<IActionResult> GetChecklist([FromForm] int weeklistId)
        {
            try
            {
                // Getting products
                List<Product> products = await _productService.GetProductsFromWeeklist(weeklistId);
                byte[] xlsBytes = _xlsFileService.GetProductChecklist(products);
                // Get weeklist to create filename
                WeeklistDto weeklist = await _weeklistService.GetWeeklistAsync(weeklistId);
                string fileName = $"{weeklist.Number}-{weeklist.OrderNumber}({weeklist.ShippingNumber})-Checklist.xls";
                return File(xlsBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Something went wrong. Please try again. {ex.Message}");
            }            
        }  

        [HttpPost("upload-checklist")]
        // [Authorize(Roles = $"{Roles.Admin}")]
        public async Task<IActionResult> UploadChecklist([FromForm] IFormFile file, [FromForm] int weeklistId)
        {
            try
            {
                var result = await _productService.UpdateProductsFromFile(file);
                if (!result.Success)
                {
                    return BadRequest(result.Message);
                }

                // Mark Current task as done, set next to ready
                var updateTaskResult = await UpdateTaskStatusAndAdvanceNext(weeklistId, WeeklistTaskNameEnum.CreateChecklist, WeeklistTaskNameEnum.InsertWarehouseList);                
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Something went wrong. Please try again. {ex.Message}");
            }
            return Ok();
        }

        [HttpPost("get-warehouse-list")]
        // [Authorize(Roles = $"{Roles.Admin}")]
        public async Task<IActionResult> GetWarehouselist([FromForm] int weeklistId)
        {
            try
            {
                List<Product> products = await _productService.GetProductsFromWeeklist(weeklistId);
                byte[] xlsBytes = _xlsFileService.GetProductWarehouselist(products);
                // Get weeklist to create filename
                WeeklistDto weeklist = await _weeklistService.GetWeeklistAsync(weeklistId);
                string fileName = $"{weeklist.Number}-Warehouselist.xls";
                // Mark Current task as done, set next to ready                
                return File(xlsBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Something went wrong. Please try again. {ex.Message}");
            }
        }  

        [HttpPost("mark-as-complete")]
        // [Authorize(Roles = $"{Roles.Admin}")]
        public async Task<IActionResult> MarkTaskAsComplete([FromForm] int weeklistId)
        {
            try
            {
                // Mark Current task as done, set next to ready
                var updateTaskResult = await UpdateTaskStatusAndAdvanceNext(weeklistId, WeeklistTaskNameEnum.InsertWarehouseList, WeeklistTaskNameEnum.ImportProductList);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Something went wrong. Please try again. {ex.Message}");
            }
        }  



    }
}
