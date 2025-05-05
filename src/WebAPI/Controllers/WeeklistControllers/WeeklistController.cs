using Application.Services.Interfaces.Weeklists;
using Domain.Constants;
using Domain.Entities.Weeklists.Entities;
using Microsoft.AspNetCore.Authorization;
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
        [Authorize]
        public async Task<IActionResult> GetAllWeeklists()
        {
            try
            {
                var weeklists = await _weeklistService.GetAllWeeklistsAsync();
                return Ok(weeklists);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Something went wrong. Please try again. {ex.Message}");
            }
        }

        [HttpPost("create")]
        [Authorize(Roles = $"{Roles.Admin},{Roles.Freelancer}")]
        public async Task<IActionResult> CreateWeeklist([FromForm] IFormFile file, [FromForm] int Number, [FromForm] string OrderNumber, [FromForm] string Supplier, [FromForm] string ShippingNumber)
        {
            try
            {
                // Instantiate weeklist object with the parameters received from the form
                var weeklist = new Weeklist
                {
                    Number = Number,
                    OrderNumber = OrderNumber,
                    Supplier = Supplier,
                    ShippingNumber = ShippingNumber
                };

                // Send to service to create the weeklist
                var result = await _weeklistService.CreateWeeklist(file, weeklist);

                // Handle the result
                if (!result.Success)
                {
                    return BadRequest(result.Message);
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

