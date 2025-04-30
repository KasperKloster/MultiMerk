using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers.TestingEndpoints
{
    [Route("api/testendpoints")]
    [ApiController]
    public class TestEndpointsController : ControllerBase
    {
        [HttpGet("debug-roles")]
        public IActionResult DebugRoles()
        {
            var roles = User.Claims
                .Where(c => c.Type == ClaimTypes.Role || c.Type == "role")
                .Select(c => c.Value);

            return Ok(new {
                IsAuthenticated = User.Identity?.IsAuthenticated,
                Roles = roles
            });
        }
    }
}
