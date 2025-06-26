using Domain.Entities.Authentication;
using Domain.Entities.Products;
using Domain.Entities.Weeklists.Entities;
using Domain.Entities.Weeklists.WeeklistTaskLinks;
using Domain.Entities.Weeklists.WeeklistTasks;
using Infrastructure.Seeder;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public class AppDbContext : IdentityDbContext<ApplicationUser>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
    
    // Authentication
    public DbSet<TokenInfo> TokenInfos { get; set; } = null!;    
    // Weeklists and tasks
    public DbSet<Weeklist> Weeklists { get; set; } = null!;
    public DbSet<WeeklistTask> WeeklistTasks { get; set; } = null!;
    public DbSet<WeeklistTaskStatus> WeeklistTaskStatus { get; set; } = null!;
    public DbSet<WeeklistTaskLink> WeeklistTaskLinks { get; set; } = null!;
    public DbSet<WeeklistTaskUserRoleAssignment> WeeklistTaskUserRoleAssignments { get; set; } = null!;
    // Products
    public DbSet<Product> Products { get; set; } = null!;
    public DbSet<ProductTemplate> ProductTemplates { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Unique index on SKU
        modelBuilder.Entity<Product>()
            .HasIndex(p => p.Sku)
            .IsUnique();

        // Unique index on WeeklistNumber
        modelBuilder.Entity<Weeklist>()
            .HasIndex(w => w.Number)
            .IsUnique();

        // Product -> Weeklist relationship
        modelBuilder.Entity<Product>()
            .HasOne(p => p.Weeklist)
            .WithMany(w => w.Products)
            .HasForeignKey(p => p.WeeklistId)
            .OnDelete(DeleteBehavior.Cascade); // or Restrict/SetNull if preferred        

        // WeeklistTaskLink Relation / Join weeklist with tasks, status
        modelBuilder.Entity<WeeklistTaskLink>()
            .HasKey(t => new { t.WeeklistId, t.WeeklistTaskId });

        modelBuilder.Entity<WeeklistTaskLink>()
            .HasOne(x => x.Weeklist)
            .WithMany(w => w.WeeklistTaskLinks)
            .HasForeignKey(x => x.WeeklistId);

        modelBuilder.Entity<WeeklistTaskLink>()
            .HasOne(x => x.WeeklistTask)
            .WithMany()
            .HasForeignKey(x => x.WeeklistTaskId);

        modelBuilder.Entity<WeeklistTaskLink>()
            .HasOne(x => x.WeeklistTaskStatus)
            .WithMany()
            .HasForeignKey(x => x.WeeklistTaskStatusId);

        // WeeklistTaskUserRoleAssignment
        modelBuilder.Entity<WeeklistTaskUserRoleAssignment>()
            .HasOne(x => x.WeeklistTask)
            .WithMany(t => t.UserRoleAssignments)
            .HasForeignKey(x => x.WeeklistTaskId)
            .OnDelete(DeleteBehavior.Cascade);

        // Templates
        modelBuilder.Entity<ProductTemplate>()
            .HasMany(t => t.Translations)
            .WithOne(tr => tr.ProductTemplate)
            .HasForeignKey(tr => tr.ProductTemplateId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<ProductTemplateTranslation>()
            .HasIndex(t => new { t.ProductTemplateId, t.LanguageCode })
            .IsUnique();

        // Seed database
        ApplySeeders(modelBuilder);
    }

    private void ApplySeeders(ModelBuilder modelBuilder)
    {           
        // Seed weeklist
        WeeklistSeeder.Seed(modelBuilder.Entity<Weeklist>());
        WeeklistTaskSeeder.Seed(modelBuilder.Entity<WeeklistTask>());
        WeeklistTaskStatusSeeder.Seed(modelBuilder.Entity<WeeklistTaskStatus>());
        WeeklistTaskLinkSeeder.Seed(modelBuilder.Entity<WeeklistTaskLink>());        
        WeeklistTaskUserRoleAssignmentSeeder.Seed(modelBuilder.Entity<WeeklistTaskUserRoleAssignment>());
        
        // // Seed products
        ProductSeeder.Seed(modelBuilder.Entity<Product>());
    }
}
