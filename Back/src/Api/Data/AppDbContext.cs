using Back.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Back.Api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options){}
    

    public DbSet<User> Users { get; set; } = null!;
    public DbSet<MediaContent> MediaContents { get; set; } = null!;
}
    