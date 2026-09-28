using System.ComponentModel.DataAnnotations;

namespace BankAccountApi.Models;

public sealed class RegisterRequest
{
    [Required, MinLength(3), MaxLength(64)]
    public string Username { get; init; } = string.Empty;

    [Required, MinLength(8), MaxLength(128)]
    public string Password { get; init; } = string.Empty;
}

public sealed class LoginRequest
{
    [Required]
    public string Username { get; init; } = string.Empty;

    [Required]
    public string Password { get; init; } = string.Empty;
}

public sealed record AuthResponse(
    string AccessToken,
    string TokenType,
    DateTime ExpiresAtUtc);

public sealed record RegisteredUserResponse(int Id, string Username);
