using Domain.Constants;
using Domain.Models.Authentication;
using Domain.Models.Authentication.DTOs;
using Infrastructure.Data;
using Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers.Authentication;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly ILogger<AuthController> _logger;
    private readonly ITokenService _tokenService;
    private readonly AppDbContext _context;

    public AuthController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, ILogger<AuthController> logger, ITokenService tokenService, AppDbContext context)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _logger = logger;
        _tokenService = tokenService;
        _context = context;
    }

    [HttpPost("signup")]
    public async Task<IActionResult> Signup(SignupModelDTO model)
    {
        try
        {
            var existingUser = await _userManager.FindByNameAsync(model.Email);
            if (existingUser != null)
            {
                return BadRequest("User already exists");
            }

            // Create User role if it doesn't exist
            if ((await _roleManager.RoleExistsAsync(Roles.User)) == false)
            {
                var roleResult = await _roleManager
                      .CreateAsync(new IdentityRole(Roles.User));

                if (roleResult.Succeeded == false)
                {
                    var roleErros = roleResult.Errors.Select(e => e.Description);
                    _logger.LogError($"Failed to create user role. Errors : {string.Join(",", roleErros)}");
                    return BadRequest($"Failed to create user role. Errors : {string.Join(",", roleErros)}");
                }
            }

            ApplicationUser user = new()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Email,
                Name = model.Name,
                EmailConfirmed = true
            };

            // Attempt to create a user
            var createUserResult = await _userManager.CreateAsync(user, model.Password);

            // Validate user creation. If user is not created, log the error and
            // return the BadRequest along with the errors
            if (createUserResult.Succeeded == false)
            {
                var errors = createUserResult.Errors.Select(e => e.Description);
                _logger.LogError(
                    $"Failed to create user. Errors: {string.Join(", ", errors)}"
                );
                return BadRequest($"Failed to create user. Errors: {string.Join(", ", errors)}");
            }

            // adding role to user
            var addUserToRoleResult = await _userManager.AddToRoleAsync(user: user, role: Roles.User);

            if (addUserToRoleResult.Succeeded == false)
            {
                var errors = addUserToRoleResult.Errors.Select(e => e.Description);
                _logger.LogError($"Failed to add role to the user. Errors : {string.Join(",", errors)}");
            }
            return CreatedAtAction(nameof(Signup), null);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }


}

