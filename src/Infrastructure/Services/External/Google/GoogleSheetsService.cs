using System;
using System.Diagnostics;
using Application.Services.Interfaces.External.Google;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Services.External.Google;

public class GoogleSheetsService : IGoogleSheetsService
{
    private readonly string _spreadsheetId;
    private readonly string _sheetName;
    private readonly SheetsService _sheetsService;
    public GoogleSheetsService(IConfiguration configuration)
    {
        _spreadsheetId = configuration["GoogleSheets:TemplateSpreadsheetId"];
        _sheetName = configuration["GoogleSheets:TemplateSheetName"];

        var credentialsPath = ResolveCredentialsPath(configuration["GoogleSheets:CredentialsPath"]);

        GoogleCredential credential;
        using (var stream = new FileStream(credentialsPath, FileMode.Open, FileAccess.Read))
        {
            credential = GoogleCredential.FromStream(stream).CreateScoped(SheetsService.Scope.SpreadsheetsReadonly);
        }

        _sheetsService = new SheetsService(new BaseClientService.Initializer
        {
            HttpClientInitializer = credential,
            ApplicationName = "MultiMerk",
        });
    }

    private static string ResolveCredentialsPath(string? configuredPath)
    {
        if (string.IsNullOrWhiteSpace(configuredPath))
            throw new InvalidOperationException("Google Sheets credentials path is missing in configuration.");

        // Move up to solution root (from /src/WebAPI to /MultiMerk)
        var rootPath = Directory.GetParent(Directory.GetParent(Directory.GetCurrentDirectory())!.FullName)!.FullName;

        string fullPath = Path.GetFullPath(Path.Combine(rootPath, configuredPath));

        if (!File.Exists(fullPath))
            throw new FileNotFoundException($"Google Sheets credentials file not found: {fullPath}");

        return fullPath;
    }

    public async Task<List<Dictionary<string, string>>> GetTemplateRowsAsync()    
    {
        var range = $"{_sheetName}!A1:Z"; // Adjust based on your columns
        var request = _sheetsService.Spreadsheets.Values.Get(_spreadsheetId, range);
        
        var response = await request.ExecuteAsync();
        var values = response.Values;

        var result = new List<Dictionary<string, string>>();

        if (values == null || values.Count == 0)
            return result;

        var headers = values[0].Select(h => h.ToString() ?? string.Empty).ToList();

        for (int i = 1; i < values.Count; i++)
        {
            var rowDict = new Dictionary<string, string>();
            var row = values[i];

            for (int j = 0; j < headers.Count; j++)
            {
                rowDict[headers[j]] = j < row.Count ? row[j]?.ToString() ?? string.Empty : string.Empty;
            }

            result.Add(rowDict);
        }

        return result;
    }
}