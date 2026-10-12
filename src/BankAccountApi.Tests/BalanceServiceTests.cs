using BankAccountApi.Models;
using BankAccountApi.Security;
using BankAccountApi.Security.Exceptions;
using BankAccountApi.Services;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace BankAccountApi.Tests;

public sealed class BalanceServiceTests
{
    [Theory]
    [InlineData("0")]
    [InlineData("-0.01")]
    public async Task Deposit_RejectsNonPositiveAmount(string value)
    {
        await using var context = CreateContext();
        var service = new BalanceService(context, new TestCurrentUser(1));

        await Assert.ThrowsAsync<BadRequestException>(() =>
            service.Deposit(new MoneyRequest { Amount = decimal.Parse(value) }));
    }

    [Theory]
    [InlineData("0")]
    [InlineData("-10")]
    public async Task Withdraw_RejectsNonPositiveAmount(string value)
    {
        await using var context = CreateContext();
        var service = new BalanceService(context, new TestCurrentUser(1));

        await Assert.ThrowsAsync<BadRequestException>(() =>
            service.Withdraw(new MoneyRequest { Amount = decimal.Parse(value) }));
    }

    private static BankAccountContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<BankAccountContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new BankAccountContext(options);
    }

    private sealed record TestCurrentUser(long Id) : ICurrentUser;
}
