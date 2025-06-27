using System;
using Application.Repositories.ApplicationUsers;
using Application.Repositories.Products;
using Application.Repositories.Weeklists;
using Application.Services.Files;
using Application.Services.Files.csv;
using Application.Services.Interfaces.External.Google;
using Application.Services.Interfaces.Files;
using Application.Services.Interfaces.Files.csv;
using Application.Services.Interfaces.Products;
using Application.Services.Interfaces.Tasks;
using Application.Services.Interfaces.Weeklists;
using Application.Services.Products;
using Application.Services.Tasks;
using Application.Services.Weeklists;
using Infrastructure.Files;
using Infrastructure.Repositories;
using Infrastructure.Repositories.ApplicationUsers;
using Infrastructure.Repositories.Products;
using Infrastructure.Repositories.Weeklists;
using Infrastructure.Services.External.Google;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        // Files
        services.AddScoped<IAICsvService, AICsvService>();
        services.AddScoped<IMagentoCsvService, MagentoCsvService>();
        services.AddScoped<IShopifyCsvService, ShopifyCsvService>();
        services.AddScoped<IFileParser, FileParser>();
        services.AddScoped<IXlsFileService, XlsFileService>();
        // Repositories
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IProductTemplateRepository, ProductTemplateRepository>();
        services.AddScoped<IWeeklistRepository, WeeklistRepository>();
        services.AddScoped<IWeeklistTaskRepository, WeeklistTaskRepository>();
        services.AddScoped<IWeeklistTaskLinkRepository, WeeklistTaskLinkRepository>();
        services.AddScoped<IWeeklistUserRoleAssignmentRepository, WeeklistUserRoleAssignmentRepository>();
        services.AddScoped<IApplicationUserRepository, ApplicationUserRepository>();
        // Services
        services.AddScoped<IWeeklistService, WeeklistService>();
        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<IProductTemplateService, ProductTemplateService>();
        services.AddScoped<IWeeklistTaskLinkService, WeeklistTaskLinkService>();
        services.AddScoped<IContentService, ContentService>();
        services.AddScoped<IWarehouseService, WarehouseService>();
        services.AddScoped<IZipService, ZipService>();
        services.AddScoped<IGoogleSheetsService, GoogleSheetsService>();

        return services;
        
    }
}
