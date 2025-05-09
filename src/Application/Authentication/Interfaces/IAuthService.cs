using Domain.Entities.Authentication.DTOs;
using Domain.Entities.Authentication;

namespace Application.Authentication.Interfaces;
public interface IAuthService
{
    Task<AuthResult> Signup(SignupModelDTO model);
    Task<AuthResult> Login(LoginModelDTO model);    
    Task<AuthResult> RefreshToken(TokenModelDTO tokenModel);    
    Task<AuthResult> RevokeToken(string username);
}