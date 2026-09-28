namespace BankAccountApi.Models;

public class BankAccountItemDTO
{
    public long Id { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
}
