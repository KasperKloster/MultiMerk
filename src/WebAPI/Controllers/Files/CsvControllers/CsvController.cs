using Application.Files.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers.Files.CsvControllers;

[Route("api/files/csv/")]
[ApiController]
public class CsvController : ControllerBase
{

    [HttpPost("upload")]
    public async Task<IActionResult> Upload(IFormFile file)
    {
        return Ok();
    }
}