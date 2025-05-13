using System.Globalization;

namespace Application.Files.Services.csv;

public class CsvBaseService
{

    protected static string Escape(string? input)
    {
        if (string.IsNullOrEmpty(input)) return "";

        if (input.Contains(',') || input.Contains('"') || input.Contains('\n'))
        {
            return $"\"{input.Replace("\"", "\"\"")}\"";
        }

        return input;
    }

    protected static string ConvertSEKToEUR(int? sek)
    {
        if (sek == null)
        {
            return "0.00";
        }

        decimal currency = 0.11m;
        decimal value = sek.Value * currency;
        decimal result = Math.Floor(value / 0.05m) * 0.05m;        
        return result.ToString("0.00", CultureInfo.InvariantCulture);
    }

    protected static int ConvertSEKToNOK(int? sek)
    {
        int valueInSek = sek ?? 0; // Use 0 if null

        double value = valueInSek + valueInSek * 0.1;
        double roundedUpTo10 = Math.Ceiling(value / 10) * 10;
        return (int)(roundedUpTo10 - 1);
    }

    protected static int ConvertSEKToDKK(int? sek)
    {
        int valueInSek = sek ?? 0;

        double discounted = valueInSek * 0.7;
        double roundedUpTo10 = Math.Ceiling(discounted / 10) * 10;
        return (int)(roundedUpTo10 - 1);
    }    
}
