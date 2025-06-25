using System.ComponentModel.DataAnnotations;

namespace Domain.Entities.Products;

public class ProductTemplate
{
    [Required]
    public int Id { get; set; }
    [Required]
    public int TemplateNumber { get; set; }

    public ICollection<ProductTemplateTranslation> Translations { get; set; } = new List<ProductTemplateTranslation>();
}
