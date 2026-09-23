using Microsoft.EntityFrameworkCore;
using Npgsql;
using Npgsql.EntityFrameworkCore.PostgreSQL;

namespace BankAccountApi.Models;

public class BankAccountContext : DbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder options) {
        var connectionString = "Host=localhost;Database=bankdb;Username=postgres;Password=";
        options.UseNpgsql(connectionString);
    }
    
    public BankAccountContext(DbContextOptions<BankAccountContext> options)
        : base(options)
    {
    }

    public DbSet<BankAccountItem> BankAccountItems { get; set; } = null!;
}
