using System;

namespace Application.Services.Interfaces.External.Google;

public interface IGoogleSheetsService
{
    // Task<List<Dictionary<string, string>>> GetTemplateRowsAsync();
    bool GetTemplateRowsAsync();

}
