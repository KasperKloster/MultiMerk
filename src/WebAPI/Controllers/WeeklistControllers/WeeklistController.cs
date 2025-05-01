using System.Diagnostics;
using Application.Services.Interfaces.Products;
using Application.Services.Interfaces.Tasks;
using Application.Services.Interfaces.Weeklists;
using Domain.Constants;
using Domain.Entities.Weeklists.Entities;
using Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers.WeeklistControllers
{
    [Route("api/weeklist/")]
    [ApiController]
    public class WeeklistController : ControllerBase
    {        
        private readonly IWeeklistService _weeklistService;
        private readonly IProductService _productService;
        private readonly IWeeklistTaskLinkService _weeklistTaskLinkService;

        public WeeklistController(IWeeklistService weeklistService, IProductService productService, IWeeklistTaskLinkService weeklistTaskLinkService)
        {
            _weeklistService = weeklistService;
            _productService = productService;
            _weeklistTaskLinkService = weeklistTaskLinkService;
        }

        [HttpGet("all")]
        [Authorize]
        public async Task<IActionResult> GetAllWeeklists()
        {
            try{
                var weeklists = await _weeklistService.GetAllWeeklistsAsync();            
                return Ok(weeklists);
            } catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("create")]
        [Authorize(Roles = $"{Roles.Admin},{Roles.Freelancer}")]
        public async Task <IActionResult> CreateWeeklist ([FromForm] IFormFile file, [FromForm] int Number, [FromForm] string OrderNumber, [FromForm] string Supplier)
        {
            // Instantiate weeklist object with the parameters received from the form
            var weeklist = new Weeklist
            {
                Number = Number,
                OrderNumber = OrderNumber,
                Supplier = Supplier
            };
            
            // Send to service to create the weeklist
            var result = await _weeklistService.CreateWeeklist(file, weeklist);            
            
            // Handle the result
            if (!result.Success) {
                return BadRequest(result.Message);
            }            
            return Ok();
        }

        [HttpPost("assign-ean")]
        // [Authorize(Roles = $"{Roles.Admin}")]
        public async Task <IActionResult> AssignEan ([FromForm] IFormFile file, [FromForm] int weeklistId)
        {                   
            // Send to service
            var result = await _productService.UpdateProductsFromFile(file);

            // // Handle the result
            if (!result.Success) {
                return BadRequest(result.Message);
            }

            // Mark Current task as done, set next task as ready
            _weeklistTaskLinkService.UpdateTaskStatus(
                weeklistId: weeklistId, 
                currentTask: WeeklistTaskName.AssignEAN, 
                taskStatus: WeeklistTaskStatus.Done);

            return Ok();
        }        
    }
}

