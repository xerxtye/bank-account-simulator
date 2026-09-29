using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BankAccountApi.Models;

/// <summary>Data required to create a bank account.</summary>
public sealed class RegisterRequest
{
    /// <summary>The account owner's first name.</summary>
    [Description("The account owner's first name.")]
    [Required, MinLength(2), MaxLength(64)]
    public string FirstName { get; init; } = string.Empty;

    /// <summary>The account owner's last name.</summary>
    [Description("The account owner's last name.")]
    [Required, MinLength(2), MaxLength(64)]
    public string LastName { get; init; } = string.Empty;

    /// <summary>A password of at least 8 characters. It is stored only as a secure hash.</summary>
    [Description("A password of at least 8 characters. It is never stored as plain text.")]
    [Required, MinLength(8), MaxLength(128)]
    public string Password { get; init; } = string.Empty;
}

/// <summary>Credentials required to obtain a JWT.</summary>
public sealed class LoginRequest
{
    /// <summary>The account ID returned by registration.</summary>
    [Description("The account ID returned by the registration endpoint.")]
    [Range(1, long.MaxValue)]
    public long AccountId { get; init; }

    /// <summary>The account password.</summary>
    [Description("The password supplied during registration.")]
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
