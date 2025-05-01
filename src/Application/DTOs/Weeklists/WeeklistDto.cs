using Application.DTOs.Products;

namespace Application.DTOs.Weeklists;

public class WeeklistDto
{    
    public int Id { get; set; }
    public int Number { get; set; }
    public string OrderNumber { get; set; } = string.Empty;
    public string Supplier { get; set; } = string.Empty;

    public List<ProductDto> Products { get; set; } = new();
    public List<WeeklistTaskLinkDto> WeeklistTasks { get; set; } = new();
}
