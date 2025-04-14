using Microsoft.AspNetCore.Http;

namespace Application.Files.Interfaces;

public interface IFileService
{
    public bool IsValidFileExtension(IFormFile file);
    public char GetDelimiterFromCsv(IFormFile file);
}