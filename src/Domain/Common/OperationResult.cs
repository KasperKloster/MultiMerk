namespace Domain.Common;

public class OperationResult
{
    public bool Success { get; set; }
    public string? Message {get; set; }

    public static OperationResult Fail(string message)
    {
        return new OperationResult {Success = false, Message = message};
    }   
    public static OperationResult Ok()
    {
        return new OperationResult { Success = true};
    }
}
