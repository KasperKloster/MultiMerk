using Domain.Entities.Files;
using Domain.Entities.Weeklists.Entities;
using Microsoft.AspNetCore.Http;

namespace Application.Files.Interfaces;

public interface IXlsFileService
{
    Task<FilesResult> CreateWeeklist(IFormFile file, Weeklist weeklist);
}
