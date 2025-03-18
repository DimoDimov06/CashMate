using CashMate.Models.Data;
using Microsoft.EntityFrameworkCore;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Purchase> Purchases { get; set; }

    public DbSet<User> Users {  get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .HasMany(u => u.Purchases)
            .WithOne(p => p.User)
            .HasForeignKey(p => p.UserID);
    }
}