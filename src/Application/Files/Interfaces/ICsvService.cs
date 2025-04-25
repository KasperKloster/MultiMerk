using Domain.Entities.Files;
using Microsoft.AspNetCore.Http;

namespace Application.Files.Interfaces;

public interface ICsvService
{
    Task<FilesResult> UploadCsv(IFormFile file);
}