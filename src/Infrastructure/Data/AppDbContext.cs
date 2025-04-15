using Domain.Models.Authentication;
using Domain.Models.Products;
using Domain.Models.Weeklists;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public class AppDbContext : IdentityDbContext<ApplicationUser>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<TokenInfo> TokenInfos { get; set; } = null!;
    public DbSet<Weeklist> Weeklists { get; set; } = null!;
    public DbSet<Product> Products { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Product -> Weeklist relationship
        modelBuilder.Entity<Product>()
            .HasOne(p => p.Weeklist)
            .WithMany(w => w.Products)
            .HasForeignKey(p => p.WeeklistId)
            .OnDelete(DeleteBehavior.Cascade); // or Restrict/SetNull if preferred
    }
}
