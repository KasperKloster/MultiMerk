using System;
using System.Diagnostics;
using Application.Repositories.Products;
using Application.Services.Interfaces.External.Google;
using Application.Services.Interfaces.Products;
using Domain.Entities.Products;
using NPOI.SS.Formula.Functions;

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

    public bool SyncTemplatesFromGoogleSheetsAsync()
    {

        _googleSheetService.GetTemplateRowsAsync();        
        return true;
    }

    // public async Task SyncTemplatesFromGoogleSheetsAsync()
    // {        
    // var sheetRows = await _googleSheetsService.GetTemplateRowsAsync();

    // var templates = new List<ProductTemplate>();

    // foreach (var row in sheetRows)
    // {
    //     var template = new ProductTemplate
    //     {
    //         TemplateNumber = int.Parse(row["Template Number"]),
    //         Translations = new List<ProductTemplateTranslation>
    //         {
    //             new() { LanguageCode = "en", Name = row["Name - English"], Description = row["Description - English"] },
    //             new() { LanguageCode = "da", Name = row["Name - Danish"], Description = row["Description - Danish"] },
    //             new() { LanguageCode = "sv", Name = row["Name - Swedish"], Description = row["Description - Swedish"] },
    //             // Add more languages here...
    //         }
    //     };

    //     templates.Add(template);
    // }

    // await _repository.UpsertTemplatesAsync(templates);
    // }
}