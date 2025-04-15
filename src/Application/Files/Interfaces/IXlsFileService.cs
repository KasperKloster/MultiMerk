using Domain.Models.Files;
using Microsoft.AspNetCore.Http;

namespace Application.Files.Interfaces;

public interface IXlsFileService
{
    Task<FilesResult> CreateWeeklist(IFormFile file);
}
