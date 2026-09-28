namespace BankAccountApi.Models;

public sealed class AppUser
{
    public int Id { get; set; }
    public required string Username { get; set; }
    public required string NormalizedUsername { get; set; }
    public required string PasswordHash { get; set; }
}
