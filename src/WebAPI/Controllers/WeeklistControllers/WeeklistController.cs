using Application.Files.Interfaces;
using Domain.Models.Weeklists;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers.WeeklistControllers
{
    [Route("api/weeklist/")]
    [ApiController]
    public class WeeklistController : ControllerBase
    {
        private readonly IXlsFileService _xlsFileService;

        public WeeklistController(IXlsFileService xlsFileService)
        {
            _xlsFileService = xlsFileService;
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
            var result = await _xlsFileService.CreateWeeklist(file, weeklist);
            
            // Handle the result
            if (!result.Success) {
                return BadRequest(result.Message);
            }            
            return Ok();
        }
    }
}

