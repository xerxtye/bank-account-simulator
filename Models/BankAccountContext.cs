using Microsoft.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore.PostgreSQL;

namespace BankAccountApi.Models;

public class BankAccountContext : DbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseNpgsql("Host=localhost;Database=bankdb;Username=postgres;Password=");
    
    public BankAccountContext(DbContextOptions<BankAccountContext> options)
        : base(options)
    {
    }

    public DbSet<BankAccountItem> BankAccountItems { get; set; } = null!;
}
