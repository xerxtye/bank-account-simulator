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
        modelBuilder.Entity<BankAccountItem>()
            .ToTable("Users");


        modelBuilder.Entity<BankAccountItem>()
            .Property(account => account.Balance)
            .HasPrecision(18, 2);
    }

    public DbSet<BankAccountItem> BankAccountItems { get; set; } = null!;
}
