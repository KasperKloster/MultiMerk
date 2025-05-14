using System.Globalization;
using System.Text;
using Application.Services.Interfaces.Files.csv;
using Domain.Entities.Products;
using Slugify;

namespace Application.Services.Files.csv;

public class ShopifyCsvService : CsvBaseService, IShopifyCsvService
{
    public byte[] GenerateShopifyDefaultImportCsv(List<Product> products)
    {
        var slug = new SlugHelper();

        var sb = new StringBuilder();
        sb.AppendLine(
            "Handle," +
            "Variant SKU," +
            "Title," +
            "Body HTML," +
            "Vendor," +
            "Variant Price," +
            "Variant Barcode," +
            "Variant Cost," +
            "Variant Weight," +
            "Unit," +
            "Image Src," +
            "Image Alt Text," +
            "Image Position," +
            "Variant Inventory Tracker," +
            "Metafield: PIM.list [single_line_text_field]," +
            "Metafield: PIM.supplier [single_line_text_field]," +
            "Metafield: PIM.supplier_sku [single_line_text_field]"

            );

        foreach (Product product in products)
        {
            sb.AppendLine(
                $"{slug.GenerateSlug(product.Title)}," +
                $"{Escape(product.Sku)}," +
                $"{Escape(product.Title)}," +
                $"{Escape(product.Description)}," +
                "Lux-case," +
                $"{ConvertSEKToEUR(product.Price)}," +
                $"{Escape(product.EAN)}," +
                $"{Escape(product.Cost?.ToString("0.00", CultureInfo.InvariantCulture) ?? "")}," +
                $"{Escape(product.Weight?.ToString("0.00", CultureInfo.InvariantCulture) ?? "")}," +
                "kg," +
                $"https://lux-case.com/media/catalog/product//0/8/{product.MainImage}," +
                $"{product.Title}," +
                "1," +
                "Shopify," +
                $"{product.Weeklist?.Number.ToString() ?? ""}," +
                $"{product.Weeklist?.Supplier ?? ""}," +
                $"{product.SupplierSku},"

            );
        }
        return Encoding.UTF8.GetBytes(sb.ToString());
    }
}
