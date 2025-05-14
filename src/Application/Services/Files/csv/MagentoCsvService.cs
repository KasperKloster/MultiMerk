using System.Globalization;
using System.Text;
using Application.Services.Interfaces.Files.csv;
using Domain.Entities.Products;

namespace Application.Services.Files.csv;

public class MagentoCsvService : CsvBaseService, IMagentoCsvService
{
    public byte[] GenerateMagentoAdminImportCsv(List<Product> products)
    {
        var sb = new StringBuilder();
        sb.AppendLine("supplier_sku;series;ean;sku;name;price;description;cost;weight;image;small_image;thumbnail;visibility;attribute_set_code;product_type;product_websites;product_online;news_from_date;options_container");
        foreach (Product product in products)
        {
            sb.AppendLine(
                $"{product.SupplierSku};" +
                $"{Escape(product.Series)};" +
                $"{Escape(product.EAN)};" +
                $"{Escape(product.Sku)};" +
                $"{Escape(product.Title)};" +
                $"{ConvertSEKToEUR(product.Price)};" +
                $"{Escape(product.Description)};" +                
                $"{Escape(product.Cost?.ToString("0.00", CultureInfo.InvariantCulture) ?? "")};"+
                $"{Escape(product.Weight?.ToString("0.00", CultureInfo.InvariantCulture) ?? "")};"+
                $"{product.MainImage};" +
                $"{product.MainImage};" +
                $"{product.MainImage};" +
                $"\"Catalog, Search\";" +
                $"\"Migration_Default\";" +
                $"\"simple\";" +
                $"\"base,se,dk,no,fi,nl,be,ie,de,ch\";" +
                $"\"1\";" +
                $"\"{DateTime.Today:MM/dd/yy}\";" +
                $"\"Block after Info Column\""
            );
            
        }
        return Encoding.UTF8.GetBytes(sb.ToString());
    }

    public byte[] GenerateMagentoAttributeImportCsv(List<Product> products)
    {
        var sb = new StringBuilder();
        sb.AppendLine("sku;categories;location;product_type");
        foreach (Product product in products)
        {
            sb.AppendLine(
                $"{Escape(product.Sku)};" +
                $"{Escape(product.CategoryId)};" +
                $"{Escape(product.Location)};" +
                $"\"simple\""
            );
        }
        return Encoding.UTF8.GetBytes(sb.ToString());
    }

    public byte[] GenerateMagentoDKKPricesImportCsv(List<Product> products)
    {
        var sb = new StringBuilder();
        sb.AppendLine("sku;price;product_type;store_view_code;product_websites");
        foreach (Product product in products)
        {
            sb.AppendLine(
                $"{Escape(product.Sku)};" +
                $"{ConvertSEKToDKK(product.Price)};" +
                $"\"simple\";" +
                $"\"dk\";" +
                $"\"dk\""
            );
        }
        return Encoding.UTF8.GetBytes(sb.ToString());
    }

    public byte[] GenerateMagentoNOKPricesImportCsv(List<Product> products)
    {
        var sb = new StringBuilder();
        sb.AppendLine("sku;price;product_type;store_view_code;product_websites");
        foreach (Product product in products)
        {
            sb.AppendLine(
                $"{Escape(product.Sku)};" +
                $"{ConvertSEKToNOK(product.Price)};" +
                $"\"simple\";" +
                $"\"no\";" +
                $"\"no\""
            );
        }
        return Encoding.UTF8.GetBytes(sb.ToString());
    }

}
