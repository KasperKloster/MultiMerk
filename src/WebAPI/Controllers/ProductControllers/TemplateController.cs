using Application.Services.Interfaces.Products;
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
    
    // [Authorize(Roles = Roles.Admin)]
    [HttpGet("sync-templates")]
    public async Task<IActionResult> SyncTemplates()    
    {
        try
        {
            await _productTemplateService.SyncTemplatesFromGoogleSheetsAsync();            
            return Ok("Templates synced successfully.");
        }
        catch (Exception ex)
        {
            return BadRequest($"Failed to sync templates: {ex.Message}");
        }
    }   
}