namespace Domain.Constants;

public static class FileExtensions
{
    public static readonly HashSet<string> Allowed = new(StringComparer.OrdinalIgnoreCase)
    {
        ".csv",
        ".xls"
    };
}
