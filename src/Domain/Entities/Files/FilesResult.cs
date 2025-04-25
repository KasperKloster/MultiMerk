namespace Domain.Entities.Files;

public class FilesResult
{
    public bool Success { get; set; }
    public string? Message {get; set; }

    public static FilesResult Fail(string message)
    {
        return new FilesResult {Success = false, Message = message};
    }   
    public static FilesResult SuccessResult()
    {
        return new FilesResult { Success = true};
    }
}