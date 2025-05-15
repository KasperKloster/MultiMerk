using Domain.Entities.Products;
namespace Domain.Entities.Files;

public class FilesResult
{
    public bool Success { get; set; }
    public string? Message { get; set; }

    public byte[] FileBytes { get; set; } = Array.Empty<byte>();
    public string FileName { get; set; } = string.Empty;

    public List<Product> Products { get; set; } = [];
    public Dictionary<string, int>? StockProducts = [];

    public static FilesResult Fail(string message)
    {
        return new FilesResult { Success = false, Message = message };
    }
    public static FilesResult SuccessResult()
    {
        return new FilesResult { Success = true };
    }
    public static FilesResult SuccessResultWithProducts(List<Product> products)
    {
        return new FilesResult { Success = true, Products = products };
    }
    public static FilesResult SuccessResultWithOutOfStock(Dictionary<string, int> stockProducts)
    {
        return new FilesResult { Success = true, StockProducts = stockProducts };
    }

    public static FilesResult SuccessWithFileExport(byte[] fileBytes, string fileName)
    {
        return new FilesResult { Success = true, FileBytes = fileBytes, FileName = fileName};
    }
}