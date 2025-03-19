using Back.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Back.Api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options){}
    

    public DbSet<User> Users { get; set; } = null!;
    public DbSet<MediaContent> MediaContents { get; set; } = null!;
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasMany(e => e.MediaContents)
                .WithOne(e => e.User)
                .HasForeignKey(e => e.UserId)
                .IsRequired();
        });
    }
}
    