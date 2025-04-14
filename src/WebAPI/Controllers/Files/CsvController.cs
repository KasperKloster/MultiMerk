using Application.Files;
using Application.Files.Interfaces;
using Microsoft.AspNetCore.Http;
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

    // [HttpPost("upload")]
    // public async Task<IActionResult> Upload(IFormFile file)
    // {
    //     // Checks if valid extension
    //     bool isFileExtensionValid = _fileService.IsValidFileExtension(file);
    //     if (!isFileExtensionValid)
    //     {
    //         return BadRequest("Filextension is not valid");
    //     }

    //     // Checks if file is .csv
    //     string FileExtension = Path.GetExtension(file.FileName);
    //     if (FileExtension != ".csv")
    //     {
    //         return BadRequest($"{file.FileName} should be an .csv file");
    //     }

    //     // Getting delimiter
    //     char delimiter = _fileService.GetDelimiterFromCsv(file);


    // }
}

