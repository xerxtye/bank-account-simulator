using Microsoft.EntityFrameworkCore;

namespace BankAccountApi.Models;

public class BankAccountContext : DbContext
{
    public BankAccountContext(DbContextOptions<BankAccountContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AppUser>()
            .HasIndex(user => user.NormalizedUsername)
            .IsUnique();
    }

    public DbSet<BankAccountItem> BankAccountItems { get; set; } = null!;
    public DbSet<AppUser> Users { get; set; } = null!;
}
