using Application.Files.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers.Files.CsvControllers;

[Route("api/files/csv/")]
[ApiController]
public class CsvController : ControllerBase
{
    private readonly ICsvService _csvService;

    public CsvController(ICsvService csvService)
    {
        _csvService = csvService;
    }

    [HttpPost("upload")]
    public async Task<IActionResult> Upload(IFormFile file)
    {
        var result = await _csvService.UploadCsv(file);        
        if (!result.Success)
        {
            return BadRequest(result.Message);
        }
        return Ok();
    }
}