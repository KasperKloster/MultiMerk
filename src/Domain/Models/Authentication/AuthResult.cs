using Domain.Models.Authentication.DTOs;

namespace Domain.Models.Authentication;

public class AuthResult
{
    public bool Success { get; set; }
    public string? Message { get; set; }
    public ApplicationUser? User { get; set; }
    public TokenModelDTO? Token  { get; set; }

    public static AuthResult Fail(string message)
    {
        return new AuthResult {Success = false, Message = message};
    }   
    public static AuthResult SuccessSignUp(ApplicationUser user)
    {
        return new AuthResult { Success = true, User = user};
    }
    public static AuthResult SuccessLogin(TokenModelDTO token)
    {
        return new AuthResult {Success = true, Token = token};
    }
    
    public static AuthResult PureSuccess()
    {
        return new AuthResult {Success = true};
    }
}