using System.ComponentModel.DataAnnotations;

namespace BankAccountApi.Models;

public sealed class MoneyRequest
{
    [Range(typeof(decimal), "0.01", "1000000000000000.00")]
    public decimal Amount { get; init; }
}

public sealed record BalanceResponse(decimal Balance);

public sealed record BalanceOperationResponse(
    string Operation,
    decimal Amount,
    decimal Balance);
