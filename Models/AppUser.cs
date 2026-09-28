namespace BankAccountApi.Models;

public sealed class AppUser
{
    public long Id { get; set; }
    public required string Username { get; set; }
    public required string NormalizedUsername { get; set; }
    public required string PasswordHash { get; set; }
    public decimal Balance { get; set; }
}
