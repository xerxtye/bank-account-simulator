using System.ComponentModel.DataAnnotations;

namespace BankAccountApi.Models;

public sealed class RegisterRequest
{

    [Required, MinLength(2), MaxLength(64)]
    public string FirstName { get; init; } = string.Empty;

    [Required, MinLength(2), MaxLength(64)]
    public string LastName { get; init; } = string.Empty;

    [Required, MinLength(8), MaxLength(128)]
    public string Password { get; init; } = string.Empty;
}

public sealed class LoginRequest
{
    [Range(1, long.MaxValue)]
    public long AccountId { get; init; }

    [Required]
    public string Password { get; init; } = string.Empty;
}

public sealed record AuthResponse(
    string AccessToken,
    string TokenType,
    DateTime ExpiresAtUtc);

public sealed record RegisteredUserResponse(
    long Id,
    string FirstName,
    string LastName);
