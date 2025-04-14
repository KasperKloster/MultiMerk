namespace Domain.Models.Products;

public class Product
{
    public Product(string sku)
    {
        Sku = sku;
    }

    public string Sku { get; set; }
}
