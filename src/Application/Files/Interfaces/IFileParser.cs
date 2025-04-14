using Microsoft.AspNetCore.Http;

namespace Application.Files.Interfaces;

public interface IFileParser
{
    char GetDelimiterFromCsv(IFormFile file);
}
