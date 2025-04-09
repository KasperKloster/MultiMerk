using System;
using Application.Authentication.Interfaces;
using Domain.Models.Authentication;
using Domain.Models.Authentication.DTOs;
using Infrastructure.Data;
using Infrastructure.Services.Token.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Services.Authentication;

public class AuthenticationService : IAuthService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly ILogger<AuthController> _logger;
    private readonly ITokenService _tokenService;
    private readonly AppDbContext _context;

    public Task Login(LoginModelDTO model)
    {
        throw new NotImplementedException();
    }

    public Task RefreshToken(TokenModelDTO tokenModel)
    {
        throw new NotImplementedException();
    }

    public Task RevokeToken()
    {
        throw new NotImplementedException();
    }

    public Task Signup(SignupModelDTO model)
    {
        throw new NotImplementedException();
    }
}

internal class AuthController
{
}