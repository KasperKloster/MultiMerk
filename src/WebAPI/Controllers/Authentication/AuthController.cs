using Application.Authentication.Interfaces;
using Domain.Models.Authentication.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers.Authentication;

[Route("api/auth/")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("signup")]
    public async Task<IActionResult> Signup(SignupModelDTO model){
        var result = await _authService.Signup(model);
        
        if (!result.Success)
        {
            return BadRequest(result.Message);
        }
        return Ok();
    }

   [HttpPost("login")]
    public async Task<IActionResult> Login(LoginModelDTO model)
    {
        var result = await _authService.Login(model);
        if (!result.Success){
            return BadRequest(result.Message);
        }

        return Ok(result.Token);
    }

    [HttpPost("token/refresh")]
    [Authorize]
    public async Task<IActionResult> RefreshToken(TokenModelDTO tokenModel)
    {
        var result = await _authService.RefreshToken(tokenModel);
                
        if (!result.Success){
            return BadRequest(result.Message);
        }

        return Ok(result.Token);
    }

    [HttpPost("token/revoke")]
    [Authorize]
    public async Task<IActionResult> RevokeToken()
    {
        // We are getting the username from token
        var username = User.Identity?.Name;
        if (string.IsNullOrEmpty(username)) {
            return BadRequest("Invalid user");
        }

        var result = await _authService.RevokeToken(username);
        if (!result.Success){
            return BadRequest(result.Message);
        }
        return Ok();
    }
}