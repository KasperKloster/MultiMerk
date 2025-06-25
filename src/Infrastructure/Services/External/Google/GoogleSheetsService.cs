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
    private readonly string _spreadsheetId = "1MKeb4zkMhwdER8gzEfJKne72m3FCQoamKcq1UeQaV2E";
    private readonly string _sheetName = "Products";
    private readonly SheetsService _sheetsService;
    public GoogleSheetsService(IConfiguration configuration)
    {
        var credentialsPath = configuration["GoogleSheets:CredentialsPath"];

        if (string.IsNullOrWhiteSpace(credentialsPath))
        {
            throw new InvalidOperationException("Missing Google Sheets credentials path configuration.");
        }

        if (!Path.IsPathRooted(credentialsPath))
        {
            // Make relative paths absolute, based on ContentRoot (project root during dev)
            var rootPath = Directory.GetCurrentDirectory();
            credentialsPath = Path.Combine(rootPath, credentialsPath);
        }

        if (!File.Exists(credentialsPath))
        {
            Console.WriteLine($"Resolved credentials path: {credentialsPath}");
            throw new FileNotFoundException($"Google Sheets credentials file not found: {credentialsPath}");
        }

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

    // public Task<List<Dictionary<string, string>>> GetTemplateRowsAsync()
    public bool GetTemplateRowsAsync()
    {
        var range = $"{_sheetName}!A1:Z"; // Adjust based on your columns
        var request = _sheetsService.Spreadsheets.Values.Get(_spreadsheetId, range);

        Debug.WriteLine("Here");
        return true;
    }
}