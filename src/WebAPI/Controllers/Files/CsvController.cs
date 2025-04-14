using Application.Files.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers.Files;

[Route("api/files/csv/")]
[ApiController]
public class CsvController : ControllerBase
{
    private readonly IFileService _fileService;

    public CsvController(IFileService fileService)
    {
        _fileService = fileService;
    }

    [HttpPost("upload")]
    public async Task<IActionResult> Upload(IFormFile file)
    {
        var result = await _fileService.UploadCsv(file);        
        if (!result.Success)
        {
            return BadRequest(result.Message);
        }
        return Ok();
    }
}