using System;

namespace Application.Services.Interfaces.Products;

public interface IProductTemplateService
{
    // Task SyncTemplatesFromGoogleSheetsAsync();
    bool SyncTemplatesFromGoogleSheetsAsync();

}
