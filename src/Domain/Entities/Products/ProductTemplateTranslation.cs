using System;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities.Products;

public class ProductTemplateTranslation
{
    [Required]
    public int Id { get; set; }
    [Required]
    public int ProductTemplateId { get; set; }
    [Required]
    public string LanguageCode { get; set; } = string.Empty; // e.g., "en", "da", "sv"
    [Required]
    public string Title { get; set; } = string.Empty;
    [Required]
    public string Description { get; set; } = string.Empty;
    [Required]
    public ProductTemplate ProductTemplate { get; set; } = null!;
}
