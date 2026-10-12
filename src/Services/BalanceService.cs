using BankAccountApi.Models;
using BankAccountApi.Security;
using BankAccountApi.Security.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace BankAccountApi.Services;


public sealed class BalanceService : IBalanceService
{
    private const decimal MaximumOperationAmount = 1_000_000_000_000_000m;

    private readonly BankAccountContext _context;
    private readonly ICurrentUser _currentUser;

    public BalanceService(BankAccountContext context, ICurrentUser currentUser)
    {
        _context = context;
        _currentUser = currentUser;
    }

    public async Task<BalanceResponse> GetBalance()
    {
        return new BalanceResponse(await GetCurrentBalance());
    }

    public async Task<BalanceOperationResponse> Deposit(MoneyRequest request)
    {
        ValidateAmount(request.Amount);

        var updatedRows = await _context.BankAccountItems
            .Where(account =>
                account.Id == _currentUser.Id &&
                account.Balance <= MaximumOperationAmount - request.Amount)
            .ExecuteUpdateAsync(setters => setters.SetProperty(
                account => account.Balance,
                account => account.Balance + request.Amount));

        if (updatedRows == 0)
        {
            if (!await _context.BankAccountItems.AnyAsync(account => account.Id == _currentUser.Id))
            {
                throw new UnauthorizedException(
                    "The user associated with this token no longer exists.");
            }

            throw new ConflictException("The maximum account balance would be exceeded.");
        }

        return new BalanceOperationResponse(
            "deposit",
            request.Amount,
            await GetCurrentBalance());
    }

    public async Task<BalanceOperationResponse> Withdraw(MoneyRequest request)
    {
        ValidateAmount(request.Amount);

        var updatedRows = await _context.BankAccountItems
            .Where(account =>
                account.Id == _currentUser.Id &&
                account.Balance >= request.Amount)
            .ExecuteUpdateAsync(setters => setters.SetProperty(
                account => account.Balance,
                account => account.Balance - request.Amount));

        if (updatedRows == 0)
        {
            if (!await _context.BankAccountItems.AnyAsync(account => account.Id == _currentUser.Id))
            {
                throw new UnauthorizedException("The user associated with this token no longer exists.");
            }

            throw new ConflictException("Insufficient funds.");
        }

        return new BalanceOperationResponse(
            "withdrawal",
            request.Amount,
            await GetCurrentBalance());
    }

    private async Task<decimal> GetCurrentBalance()
    {
        var balance = await _context.BankAccountItems
            .Where(account => account.Id == _currentUser.Id)
            .Select(account => (decimal?)account.Balance)
            .SingleOrDefaultAsync();

        return balance
            ?? throw new UnauthorizedException(
                "The user associated with this token no longer exists.");
    }

    private static void ValidateAmount(decimal amount)
    {
        if (amount <= 0 || amount > MaximumOperationAmount)
        {
            throw new BadRequestException(
                $"Amount must be greater than zero and no more than {MaximumOperationAmount}.");
        }
    }

}
