using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BankAccountApi.Models;

/// <summary>A request to change the current account balance.</summary>
public sealed class MoneyRequest
{
    /// <summary>The positive amount to deposit or withdraw.</summary>
    [Description("A positive monetary amount between 0.01 and 1000000000000000.00.")]
    [Range(typeof(decimal), "0.01", "1000000000000000.00")]
    public decimal Amount { get; init; }
}

public sealed record BalanceResponse(decimal Balance);

public sealed record BalanceOperationResponse(
    string Operation,
    decimal Amount,
    decimal Balance);
