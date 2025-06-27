using System;
using Application.Repositories.Products;
using Application.Services.Interfaces.External.Google;
using Application.Services.Interfaces.Products;
using Domain.Entities.Products;

namespace Application.Services.Products;
public class ProductTemplateService : IProductTemplateService
{
    private readonly IProductTemplateRepository _repository;
    private readonly IGoogleSheetsService _googleSheetService;

    public ProductTemplateService(IProductTemplateRepository repository, IGoogleSheetsService googleSheetService)
    {
        _repository = repository;
        _googleSheetService = googleSheetService;
    }

    public async Task SyncTemplatesFromGoogleSheetsAsync()
    {        
        var sheetRows = await _googleSheetService.GetTemplateRowsAsync();
        var templates = new List<ProductTemplate>();

        foreach (var row in sheetRows)
        {
            var template = new ProductTemplate
            {
                TemplateNumber = int.Parse(row["Template Number"]),
                Translations = new List<ProductTemplateTranslation>
                {
                    new() { LanguageCode = "en", Title = row["Name - English"], Description = row["Description - English"] },
                    new() { LanguageCode = "da", Title = row["Name - Danish"], Description = row["Description - Danish"] },
                    new() { LanguageCode = "sv", Title = row["Name - Swedish"], Description = row["Description - Swedish"] },
                    new() { LanguageCode = "no", Title = row["Name - Norwegian"], Description = row["Description - Norwegian"] },
                    new() { LanguageCode = "fi", Title = row["Name - Finnish"], Description = row["Description - Finnish"] },
                    new() { LanguageCode = "nl", Title = row["Name - Dutch"], Description = row["Description - Dutch"] },
                    new() { LanguageCode = "de", Title = row["Name - German"], Description = row["Description - German"] },
                    new() { LanguageCode = "fr", Title = row["Name - French"], Description = row["Description - French"] },
                    new() { LanguageCode = "it", Title = row["Name - Italian"], Description = row["Description - Italian"] },
                    new() { LanguageCode = "pl", Title = row["Name - Polish"], Description = row["Description - Polish"] },
                    new() { LanguageCode = "es", Title = row["Name - Spanish"], Description = row["Description - Spanish"] },                    
                }
            };

            templates.Add(template);
        }

        await _repository.UpsertTemplatesAsync(templates);
    }
}