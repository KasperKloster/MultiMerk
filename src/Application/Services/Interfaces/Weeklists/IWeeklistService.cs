using System;
using Application.DTOs.Weeklists;
using Domain.Entities.Files;
using Domain.Entities.Weeklists.Entities;
using Microsoft.AspNetCore.Http;

namespace Application.Services.Interfaces.Weeklists;

public interface IWeeklistService
{
    Task<FilesResult> CreateWeeklist(IFormFile file, Weeklist weeklist);
    Task<List<WeeklistDto>> GetAllWeeklistsAsync();
    Task<WeeklistDto> GetWeeklistAsync(int weeklistId);
}
