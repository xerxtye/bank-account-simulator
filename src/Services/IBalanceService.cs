using BankAccountApi.Models;

namespace BankAccountApi.Services;

public interface IBalanceService
{
    Task<BalanceResponse> GetBalance();
    Task<BalanceOperationResponse> Deposit(MoneyRequest request);
    Task<BalanceOperationResponse> Withdraw(MoneyRequest request);
}
