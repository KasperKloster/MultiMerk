using System;
using Domain.Entities.Products;

namespace Application.Repositories.Products;

public interface IProductTemplateRepository
{
    Task<List<ProductTemplate>> GetAllAsync();
    Task<ProductTemplate?> GetByIdAsync(int id);
    Task UpsertTemplatesAsync(List<ProductTemplate> templates);

    Task<string?> GetTemplateTitleAsync(int templateId, string? languageCode = "en");
    Task<string?> GetTemplateDescriptionAsync(int templateId, string? languageCode = "en");
}
