namespace BankAccountApi.Models;

public sealed class BankAccountItem
{
    public long Id { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string PasswordHash { get; set; }
    public decimal Balance { get; set; }
}
