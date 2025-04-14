using System.ComponentModel;
using Application.Files.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers.Files;

[Route("api/files/")]
[ApiController]
public class FileController : ControllerBase
{
    private readonly IFileService _fileService;

    public FileController(IFileService fileService)
    {
        _fileService = fileService;
    }

    [HttpPost("upload")]
    public void Upload()
    {
        string getFileType = _fileService.DetermineFileType();            
    }
}
