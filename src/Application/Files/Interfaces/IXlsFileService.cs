using Domain.Models.Files;
using Domain.Models.Weeklists.Entities;
using Microsoft.AspNetCore.Http;

namespace Application.Files.Interfaces;

public interface IXlsFileService
{
    Task<FilesResult> CreateWeeklist(IFormFile file, Weeklist weeklist);
}
