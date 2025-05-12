using System.ComponentModel.DataAnnotations;
using Domain.Entities.Products;
using Domain.Entities.Weeklists.WeeklistTaskLinks;

namespace Domain.Entities.Weeklists.Entities;
public class Weeklist
{
    [Required]
    public int Id { get; set; }    
    [Required]    
    public int Number { get; set; }        
    public string OrderNumber { get; set; } = string.Empty;        
    public string Supplier { get; set; } = string.Empty;
    public string ShippingNumber { get; set; } = string.Empty;
    // Relationsship
    // Has many products
    public ICollection<Product> Products { get; set; } = new List<Product>();
    // Join table for Tasks, and status
    public ICollection<WeeklistTaskLink> WeeklistTaskLinks{ get; set; } = new List<WeeklistTaskLink>();
}
