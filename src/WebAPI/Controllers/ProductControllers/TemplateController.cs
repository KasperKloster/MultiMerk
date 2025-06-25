using Application.Services.Interfaces.Products;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers.ProductControllers;
[Route("api/products/templates")]
[ApiController]
public class TemplateController : ControllerBase
{
    private readonly IProductTemplateService _productTemplateService;

    public TemplateController(IProductTemplateService productTemplateService)
    {
        _productTemplateService = productTemplateService;
    }

    // [HttpPost("sync-templates")]
    [HttpGet("sync-templates")]
    // [Authorize(Roles = Roles.Admin)]
    // public async Task<IActionResult> SyncTemplates()
    public IActionResult SyncTemplates()
    {
        try
        {
            _productTemplateService.SyncTemplatesFromGoogleSheetsAsync();
            // await _productTemplateService.SyncTemplatesFromGoogleSheetsAsync();
            return Ok("Templates synced successfully.");
        }
        catch (Exception ex)
        {
            return BadRequest($"Failed to sync templates: {ex.Message}");
        }
    }   
}