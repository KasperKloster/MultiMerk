using Application.Files.Interfaces;
using Application.Services.Interfaces.Weeklists;
using Application.Services.Weeklists;
using Domain.Entities.Weeklists.Entities;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers.WeeklistControllers
{
    [Route("api/weeklist/")]
    [ApiController]
    public class WeeklistController : ControllerBase
    {        
        private readonly IWeeklistService _weeklistService;
        public WeeklistController(IWeeklistService weeklistService)
        {
            _weeklistService = weeklistService;
        }

        [HttpGet("all")]
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
    }
}

