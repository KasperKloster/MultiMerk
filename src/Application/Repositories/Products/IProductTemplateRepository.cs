using System;
using Domain.Entities.Products;

namespace Application.Repositories.Products;

public interface IProductTemplateRepository
{
    Task<List<ProductTemplate>> GetAllAsync();
    Task<ProductTemplate?> GetByIdAsync(int id);
    Task UpsertTemplatesAsync(List<ProductTemplate> templates);
}
