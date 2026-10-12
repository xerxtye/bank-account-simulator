namespace BankAccountApi.Models;

/// <summary>Public bank account data. Sensitive fields are intentionally omitted.</summary>
public class BankAccountItemDTO
{
    /// <summary>The account ID used for login.</summary>
    public long Id { get; set; }

    /// <summary>The account owner's first name.</summary>
    public required string FirstName { get; set; }

    /// <summary>The account owner's last name.</summary>
    public required string LastName { get; set; }
}
