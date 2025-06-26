using Application.Repositories.Products;
using Domain.Entities.Products;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Products;

public class ProductTemplateRepository(AppDbContext dbContext) : IProductTemplateRepository
{
    private readonly AppDbContext _dbContext = dbContext;

    public async Task<List<ProductTemplate>> GetAllAsync()
    {
        return await _dbContext.ProductTemplates
            .Include(t => t.Translations)
            .ToListAsync();
    }

    public async Task<ProductTemplate?> GetByIdAsync(int id)
    {
        return await _dbContext.ProductTemplates
        .Include(t => t.Translations)
        .FirstOrDefaultAsync(t => t.Id == id);
    }

    public async Task UpsertTemplatesAsync(List<ProductTemplate> templates)
    {
        foreach (var template in templates)
        {
            var existing = await _dbContext.ProductTemplates
                .Include(t => t.Translations)
                .FirstOrDefaultAsync(t => t.TemplateNumber == template.TemplateNumber);

            if (existing == null)
            {
                _dbContext.ProductTemplates.Add(template);
            }
            else
            {
                existing.Translations.Clear();
                foreach (var translation in template.Translations)
                {
                    existing.Translations.Add(translation);
                }
            }
        }

        await _dbContext.SaveChangesAsync();
    }

}