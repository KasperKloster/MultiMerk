using System;
using Domain.Models.Authentication.DTOs;

namespace Application.Authentication.Interfaces;

public interface IAuthService
{
    Task Signup(SignupModelDTO model);
    Task Login(LoginModelDTO model);
    Task RefreshToken(TokenModelDTO tokenModel);
    Task RevokeToken();
}
