using Domain.Models.Authentication;
using Domain.Models.Products;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public class AppDbContext : IdentityDbContext<ApplicationUser>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<TokenInfo> TokenInfos { get; set; } = null!;

    public DbSet<Product> Products { get; set; } = null!;
}
