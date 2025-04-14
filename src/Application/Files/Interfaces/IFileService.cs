using Domain.Models.Files;
using Microsoft.AspNetCore.Http;

namespace Application.Files.Interfaces;

public interface IFileService
{
    Task<FilesResult> UploadCsv(IFormFile file);
}