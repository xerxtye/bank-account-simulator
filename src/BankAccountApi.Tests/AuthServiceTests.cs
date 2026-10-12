using Xunit;
using BankAccountApi.Models;
using BankAccountApi.Security;
using BankAccountApi.Services;
using Microsoft.EntityFrameworkCore;

using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace BankAccountApi.BankAccountApi.Tests;

public sealed class AuthServiceTests
{
    [Fact]
    public async Task Register_HashesPasswordAndTrimsNames()
    {
        await using var context = CreateContext();
        var service = CreateService(context);

        var result = await service.Register(new RegisterRequest
        {
            FirstName = "  Alice  ",
            LastName = "  Smith  ",
            Password = "secure-password"
        });

        var account = await context.BankAccountItems.SingleAsync();

        Assert.Equal(account.Id, result.Id);
        Assert.Equal("Alice", account.FirstName);
        Assert.Equal("Smith", account.LastName);
        Assert.NotEqual("secure-password", account.PasswordHash);
        Assert.NotEmpty(account.PasswordHash);
        Assert.Equal(0m, account.Balance);
    }

    [Fact]
    public async Task Login_ReturnsTokenForValidAccountIdAndPassword()
    {
        await using var context = CreateContext();
        var service = CreateService(context);
        var registered = await service.Register(new RegisterRequest
        {
            FirstName = "Alice",
            LastName = "Smith",
            Password = "secure-password"
        });

        var result = await service.Login(new LoginRequest
        {
            AccountId = registered.Id,
            Password = "secure-password"
        });

        Assert.Equal("Bearer", result.TokenType);
        Assert.NotEmpty(result.AccessToken);
        Assert.True(result.ExpiresAtUtc > DateTime.UtcNow);
    }

    [Fact]
    public async Task Login_RejectsWrongPassword()
    {
        await using var context = CreateContext();
        var service = CreateService(context);
        var registered = await service.Register(new RegisterRequest
        {
            FirstName = "Alice",
            LastName = "Smith",
            Password = "secure-password"
        });

        await Assert.ThrowsAsync<Security.Exceptions.UnauthorizedException>(() =>
            service.Login(new LoginRequest
            {
                AccountId = registered.Id,
                Password = "wrong-password"
            }));
    }

    private static BankAccountContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<BankAccountContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new BankAccountContext(options);
    }

    private static AuthService CreateService(BankAccountContext context)
    {
        var jwtOptions = Options.Create(new JwtOptions
        {
            Issuer = "BankAccountApi.Tests",
            Audience = "BankAccountApi.Tests.Client",
            Secret = "test-signing-secret-at-least-32-characters",
            ExpirationMinutes = 5
        });

        return new AuthService(
            context,
            new PasswordHasher<BankAccountItem>(),
            jwtOptions);
    }
}
