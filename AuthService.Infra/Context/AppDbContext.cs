using AuthService.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Infra.Context;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
    
    public DbSet<LoginCredential> Credentials { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<LoginCredential>().HasKey(p => p.Id);
    }
}