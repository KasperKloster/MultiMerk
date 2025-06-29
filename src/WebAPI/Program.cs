using System.Security.Claims;
using System.Text;
using Application.Authentication.Interfaces;
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
using Domain.Entities.Authentication;
using Infrastructure.Data;
using Infrastructure.Files;
using Infrastructure.Repositories;
using Infrastructure.Repositories.ApplicationUsers;
using Infrastructure.Repositories.Products;
using Infrastructure.Repositories.Weeklists;
using Infrastructure.Seeder;
using Infrastructure.Services.Authentication;
using Infrastructure.Services.External.Google;
using Infrastructure.Services.Token;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;

// var builder = WebApplication.CreateBuilder(args);
var builder = WebApplication.CreateBuilder(new WebApplicationOptions
{
  Args = args,
  ContentRootPath = Directory.GetCurrentDirectory()

});

builder.Configuration
  .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
  .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
  .AddEnvironmentVariables();

// Add services to the container.
string connectionString = builder.Configuration.GetConnectionString("default")!;

builder.Services.AddDbContext<AppDbContext>(options => options.UseNpgsql(connectionString));

// For Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();

// Authentication
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IAuthService, AuthService>();

var jwtSecret = builder.Configuration["JWT:secret"] ?? throw new InvalidOperationException("JWT secret is not configured.");

builder.Services.AddAuthentication(options =>
    {
      options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
      options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
      options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    }
 )
   .AddJwtBearer(options =>
     {
       options.SaveToken = true;
       options.RequireHttpsMetadata = false;
       options.TokenValidationParameters = new TokenValidationParameters
       {
         ValidateIssuer = true,
         ValidateAudience = true,
         ValidAudience = builder.Configuration["JWT:ValidAudience"],
         ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
         ClockSkew = TimeSpan.Zero,
         IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret)),
         RoleClaimType = ClaimTypes.Role
       };
     }
    );

// Files
builder.Services.AddScoped<IAICsvService, AICsvService>();
builder.Services.AddScoped<IMagentoCsvService, MagentoCsvService>();
builder.Services.AddScoped<IShopifyCsvService, ShopifyCsvService>();
builder.Services.AddScoped<IFileParser, FileParser>();
builder.Services.AddScoped<IXlsFileService, XlsFileService>();
// Repositories
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductTemplateRepository, ProductTemplateRepository>();
builder.Services.AddScoped<IWeeklistRepository, WeeklistRepository>();
builder.Services.AddScoped<IWeeklistTaskRepository, WeeklistTaskRepository>();
builder.Services.AddScoped<IWeeklistTaskLinkRepository, WeeklistTaskLinkRepository>();
builder.Services.AddScoped<IWeeklistUserRoleAssignmentRepository, WeeklistUserRoleAssignmentRepository>();
builder.Services.AddScoped<IApplicationUserRepository, ApplicationUserRepository>();
// Services
builder.Services.AddScoped<IWeeklistService, WeeklistService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IProductTemplateService, ProductTemplateService>();
builder.Services.AddScoped<IWeeklistTaskLinkService, WeeklistTaskLinkService>();
builder.Services.AddScoped<IContentService, ContentService>();
builder.Services.AddScoped<IWarehouseService, WarehouseService>();
builder.Services.AddScoped<IZipService, ZipService>();
builder.Services.AddScoped<IGoogleSheetsService, GoogleSheetsService>();


// Allow CORS for your frontend
builder.Services.AddCors(options =>
{
  options.AddPolicy("AllowMultiMerkFrontend", policy =>
  {
    policy.WithOrigins("http://localhost:5173") // Vue dev server
            .AllowAnyHeader()
            .AllowAnyMethod()
            .WithExposedHeaders("Content-Disposition");;
  });
});


builder.Services.AddAuthorization();
builder.Services.AddControllers();
builder.Services.AddOpenApi();

var app = builder.Build();
// Redirect HTTP to HTTPS before any auth logic
app.UseHttpsRedirection();
// CORS must come before authentication
app.UseCors("AllowMultiMerkFrontend");
// Authentication before authorization
app.UseAuthentication();
app.UseAuthorization();
// Only in development: enable Swagger and Scalar API docs
if (app.Environment.IsDevelopment())
{
  app.MapOpenApi();
  app.MapScalarApiReference();  
  
}
// Controllers (routing)
app.MapControllers();
// Optional test endpoint
app.MapGet("/", () => "Hello World!");

// Migrate database at startup
using (var scope = app.Services.CreateScope())
{
  var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
  await dbContext.Database.MigrateAsync();
}


// Seed initial user data
await DbSeeder.SeedData(app);

app.Run();

// Needed in order to run tests
public partial class Program { }