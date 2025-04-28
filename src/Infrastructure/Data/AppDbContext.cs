using Domain.Entities.Authentication;
using Domain.Entities.Products;
using Domain.Entities.Weeklists.Entities;
using Domain.Entities.Weeklists.WeeklistTaskLinks;
using Domain.Entities.Weeklists.WeeklistTasks;
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
    public DbSet<WeeklistTaskAssignment> WeeklistTaskAssignments { get; set; } = null!;
    
    // Products
    public DbSet<Product> Products { get; set; } = null!;

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
        
        // Seed Weeklisttasks
        modelBuilder.Entity<WeeklistTask>().HasData(
            new WeeklistTask { Id = 1, Name = "Assign EAN" },
            new WeeklistTask { Id = 2, Name = "Create AI content list" },
            new WeeklistTask { Id = 3, Name = "Assign location" },
            new WeeklistTask { Id = 4, Name = "Assign correct quantity" },
            new WeeklistTask { Id = 5, Name = "Upload AI content" },
            new WeeklistTask { Id = 6, Name = "Create final list" },
            new WeeklistTask { Id = 7, Name = "Import product list" },
            new WeeklistTask { Id = 8, Name = "Create translations" }
        );

        // Seed WeeklistTasksStatus
        modelBuilder.Entity<WeeklistTaskStatus>().HasData(
            new WeeklistTaskStatus { Id = 1, Status = "Awaiting" },
            new WeeklistTaskStatus { Id = 2, Status = "Ready" },
            new WeeklistTaskStatus { Id = 3, Status = "In Progress" },
            new WeeklistTaskStatus { Id = 4, Status = "Done" }
        );           

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

        // WeeklistTaskAssignment: User to WeeklistTask
        modelBuilder.Entity<WeeklistTaskAssignment>()
            .HasKey(wta => new { wta.ApplicationUserId, wta.WeeklistTaskId });

        // Optional: relationships
        modelBuilder.Entity<WeeklistTaskAssignment>()
            .HasOne(wta => wta.ApplicationUser)
            .WithMany(u => u.WeeklistTaskAssignments)
            .HasForeignKey(wta => wta.ApplicationUserId);

        modelBuilder.Entity<WeeklistTaskAssignment>()
            .HasOne(wta => wta.WeeklistTask)
            .WithMany(t => t.AssignedUsers)
            .HasForeignKey(wta => wta.WeeklistTaskId);        
    }
}
