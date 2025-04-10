using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Application.Authentication.Interfaces;
using Domain.Constants;
using Domain.Models.Authentication;
using Domain.Models.Authentication.DTOs;
using Infrastructure.Data;
using Infrastructure.Services.Token.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Services.Authentication;

public class AuthService : IAuthService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly ITokenService _tokenService;
    private readonly AppDbContext _context;

    public AuthService(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, ITokenService tokenService, AppDbContext context)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _tokenService = tokenService;
        _context = context;
    }

    public async Task<AuthResult> Signup(SignupModelDTO model)
    {
        try
        {  
            // Trim and normalize case before checking
            string trimmedUsername = model.Username.Trim().ToLower();          
            string trimmedEmail = model.Email.Trim().ToLower();
            
            // Check if any user has BOTH the same username and same email (Multiple users with the same email)
            var existingUser = _userManager.Users.FirstOrDefault(u => u.UserName != null && u.UserName.ToLower() == trimmedUsername && u.Email!= null && u.Email.ToLower() == trimmedEmail);
            if (existingUser != null)
            {
                return AuthResult.Fail("User already exists");                
            }

            // Create User role if it doesn't exist
            if ((await _roleManager.RoleExistsAsync(Roles.User)) == false)
            {
                var roleResult = await _roleManager
                      .CreateAsync(new IdentityRole(Roles.User));

                if (roleResult.Succeeded == false)
                {
                    var roleErrors = roleResult.Errors.Select(e => e.Description);
                    return AuthResult.Fail($"Failed to create role: {roleErrors}");
                }
            }
            
            // New user
            ApplicationUser user = new()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Username,
                Name = model.Username,
                EmailConfirmed = true
            };

            // Attempt to create a user
            var createUserResult = await _userManager.CreateAsync(user, model.Password);

            // Validate user creation. If user is not created, log the error and            
            if (createUserResult.Succeeded == false)
            {
                var errors = createUserResult.Errors.Select(e => e.Description);
                return AuthResult.Fail($"Failed to create user. Errors: {string.Join(", ", errors)}");                
            }

            // Adding role to user
            var addUserToRoleResult = await _userManager.AddToRoleAsync(user: user, role: Roles.User);

            if (addUserToRoleResult.Succeeded == false)
            {
                var errors = addUserToRoleResult.Errors.Select(e => e.Description);
                return AuthResult.Fail($"Failed to add role to the user. Errors : {string.Join(",", errors)}");
            }
            
            // All is good
            return AuthResult.SuccessSignUp(user: user);            
        }
        catch (Exception ex)
        {
            return AuthResult.Fail($"Unexpected error: {ex.Message}");                        
        }
    }
    
    public async Task<AuthResult> Login(LoginModelDTO model)
    {
        try
        {
            var user = await _userManager.FindByNameAsync(model.Username);
            if (user == null)
            {
                return AuthResult.Fail(message: "User with this username is not registered with us.");                
            }

            bool isValidPassword = await _userManager.CheckPasswordAsync(user, model.Password);
            if (isValidPassword == false)
            {
                return AuthResult.Fail(message: "Password is not valid");                
            }

            // Creating the necessary claims
            List<Claim> authClaims = [
                new (ClaimTypes.Name, user.UserName ?? string.Empty),
                new Claim(ClaimTypes.Email, user.Email ?? string.Empty),
                new (JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()), // unique id for toke                
            ];
                
            var userRoles = await _userManager.GetRolesAsync(user);

            // Adding roles to the claims. So that we can get the user role from the token.
            foreach (var userRole in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, userRole));
            }

            // Generating access token
            var token = _tokenService.GenerateAccessToken(authClaims);

            string refreshToken = _tokenService.GenerateRefreshToken();

            // Save refreshToken with exp date in the database
            var tokenInfo = _context.TokenInfos.
                        FirstOrDefault(a => a.Username == user.UserName);

            // If tokenInfo is null for the user, create a new one
            if (tokenInfo == null)
            {
                var ti = new TokenInfo
                {
                    Username = user.UserName,
                    RefreshToken = refreshToken,
                    ExpiredAt = DateTime.UtcNow.AddDays(7)
                };
                _context.TokenInfos.Add(ti);
            }
            // Else, update the refresh token and expiration
            else
            {
                tokenInfo.RefreshToken = refreshToken;
                tokenInfo.ExpiredAt = DateTime.UtcNow.AddDays(7);
            }

            // Save to DB
            await _context.SaveChangesAsync();

            // Return success
            return AuthResult.SuccessLogin(token: new TokenModelDTO{ AccessToken = token, RefreshToken = refreshToken});
        }
        catch (Exception ex)
        {
            return AuthResult.Fail(message: $"Unexpected error: {ex.Message}");
        }
    }

    public async Task<AuthResult> RefreshToken(TokenModelDTO tokenModel)
    {
        try
        {
            var principal = _tokenService.GetPrincipalFromExpiredToken(tokenModel.AccessToken);
            var username = principal.Identity.Name;

            var tokenInfo = _context.TokenInfos.SingleOrDefault(u => u.Username == username);
            if (tokenInfo == null || tokenInfo.RefreshToken != tokenModel.RefreshToken || tokenInfo.ExpiredAt <= DateTime.UtcNow)
            {
                return AuthResult.Fail("Invalid refresh token. Please login again.");                
            }

            var newAccessToken = _tokenService.GenerateAccessToken(principal.Claims);
            var newRefreshToken = _tokenService.GenerateRefreshToken();

            tokenInfo.RefreshToken = newRefreshToken; // rotating the refresh token
            // Save to DB
            await _context.SaveChangesAsync();
            
            // Return Token
            return AuthResult.SuccessLogin(token: new TokenModelDTO {AccessToken = newAccessToken, RefreshToken = newRefreshToken});            
        }
        catch (Exception ex)
        {
            return AuthResult.Fail($"Unexpected error: {ex.Message}");
        }
    }

    public async Task<AuthResult> RevokeToken(string username)
    {        
        try
        {            
            var user = _context.TokenInfos.SingleOrDefault(u => u.Username == username);
            if (user == null)
            {
                return AuthResult.Fail("User not found");
            }

            user.RefreshToken = string.Empty;
            await _context.SaveChangesAsync();

            return AuthResult.PureSuccess();
        }
        catch (Exception ex)
        {
            return AuthResult.Fail($"Error: {ex.Message}");            
        }
    }
}