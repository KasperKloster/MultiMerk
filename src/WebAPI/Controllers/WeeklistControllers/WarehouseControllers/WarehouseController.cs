using Application.Services.Interfaces.Weeklists;
using Domain.Constants;
using Domain.Entities.Files;
using Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers.WeeklistControllers.WarehouseControllers
{
    [Route("api/weeklist/warehouse/")]
    [ApiController]
    public class WarehouseController : ControllerBase
    {
        private readonly IWarehouseService _warehouseService;
        public WarehouseController(IWarehouseService warehouseService)
        {
            _warehouseService = warehouseService;
        }

        [HttpPost("get-checklist")]
        [Authorize(Roles = $"{Roles.Admin},{Roles.WarehouseManager}, {Roles.WarehouseWorker}")]
        public async Task<IActionResult> GetChecklist([FromForm] int weeklistId)
        {
            try
            {
                FilesResult result = await _warehouseService.GetChecklist(weeklistId);
                if (!result.Success)
                {
                    return BadRequest();
                }

                return File(result.FileBytes, "application/vnd.ms-excel", result.FileName);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("upload-checklist")]
        [Authorize(Roles = $"{Roles.Admin},{Roles.WarehouseManager}, {Roles.WarehouseWorker}")]
        public async Task<IActionResult> UploadChecklist([FromForm] IFormFile file, [FromForm] int weeklistId)
        {
            try
            {
                FilesResult result = await _warehouseService.UploadChecklistAndTaskAdvance(
                    file,
                    weeklistId,
                    TaskNameEnum.CreateChecklist,
                    TaskNameEnum.InsertWarehouseList);

                if (!result.Success)
                {
                    return BadRequest(result.Message);
                }
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("get-warehouse-list")]
        [Authorize(Roles = $"{Roles.Admin},{Roles.WarehouseManager}, {Roles.WarehouseWorker}")]
        public async Task<IActionResult> GetWarehouselist([FromForm] int weeklistId)
        {
            try
            {
                FilesResult result = await _warehouseService.GetWarehouselist(weeklistId);
                if (!result.Success)
                {
                    return BadRequest();
                }

                return File(result.FileBytes, "application/vnd.ms-excel", result.FileName);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("mark-as-complete")]
        [Authorize(Roles = $"{Roles.Admin},{Roles.WarehouseManager}")]
        public async Task<IActionResult> MarkTaskAsComplete([FromForm] int weeklistId)
        {
            try
            {
                // Mark Current task as done, set next to ready
                FilesResult result = await _warehouseService.MarkCompleteAdvanceNext(
                    weeklistId,
                    TaskNameEnum.InsertWarehouseList,
                    TaskNameEnum.ImportProductList);
                if (!result.Success)
                {
                    return BadRequest(result.Message);
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
