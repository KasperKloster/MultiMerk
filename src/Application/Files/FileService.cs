using Application.Files.Interfaces;
using Domain.Constants;
using Microsoft.AspNetCore.Http;


namespace Application.Files;

public class FileService : IFileService
{    
    private readonly IFileParser _fileparser;

    public FileService(IFileParser fileparser)
    {
        _fileparser = fileparser;
    }

    public bool IsValidFileExtension(IFormFile file)
    {
        string FileExtension = Path.GetExtension(file.FileName);
        return FileExtensions.Allowed.Contains(FileExtension);
    }

    public char GetDelimiterFromCsv(IFormFile file)
    {
        char delimiter = _fileparser.GetDelimiterFromCsv(file);
        return delimiter;
    }

}