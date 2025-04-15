using Application.Files.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers.Files.XlsControllers
{
    [Route("api/files/weeklist/")]
    [ApiController]
    public class WeeklistController : ControllerBase
    {
        private readonly IXlsFileService _xlsFileService;

        public WeeklistController(IXlsFileService xlsFileService)
        {
            _xlsFileService = xlsFileService;
        }

        [HttpPost("create")]
        public async Task <IActionResult> CreateWeeklist(IFormFile file)
        {
            var result = await _xlsFileService.CreateWeeklist(file);
            if (!result.Success) {
                return BadRequest(result.Message);
            }            
            return Ok();
        }
    }
}

