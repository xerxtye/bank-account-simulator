using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BankAccountApi.Models;
using BankAccountApi.Security;
using BankAccountApi.Security.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Npgsql;

namespace BankAccountApi.Services;

public sealed class AuthService : IAuthService
{
    private readonly BankAccountContext _context;
    private readonly IPasswordHasher<AppUser> _passwordHasher;
    private readonly JwtOptions _jwtOptions;

    public AuthService(
        BankAccountContext context,
        IPasswordHasher<AppUser> passwordHasher,
        IOptions<JwtOptions> jwtOptions)
    {
        _context = context;
        _passwordHasher = passwordHasher;
        _jwtOptions = jwtOptions.Value;
    }

    public async Task<RegisteredUserResponse> Register(RegisterRequest request)
    {
        var username = request.Username.Trim();
        if (username.Length < 3)
        {
            throw new BadRequestException("Username must contain at least 3 non-whitespace characters.");
        }

        var normalizedUsername = NormalizeUsername(username);

        if (await _context.Users.AnyAsync(user => user.NormalizedUsername == normalizedUsername))
        {
            throw new ConflictException("A user with this username already exists.");
        }

        var user = new AppUser
        {
            Username = username,
            NormalizedUsername = normalizedUsername,
            PasswordHash = string.Empty
        };

        user.PasswordHash = _passwordHasher.HashPassword(user, request.Password);

        _context.Users.Add(user);

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException exception)
            when (exception.InnerException is PostgresException
            {
                SqlState: PostgresErrorCodes.UniqueViolation
            })
        {
            throw new ConflictException("A user with this username already exists.");
        }

        return new RegisteredUserResponse(user.Id, user.Username);
    }

    public async Task<AuthResponse> Login(LoginRequest request)
    {
        var normalizedUsername = NormalizeUsername(request.Username);
        var user = await _context.Users.SingleOrDefaultAsync(
            candidate => candidate.NormalizedUsername == normalizedUsername);

        if (user is null)
        {
            throw new UnauthorizedException("Invalid username or password.");
        }

        var verificationResult = _passwordHasher.VerifyHashedPassword(
            user,
            user.PasswordHash,
            request.Password);

        if (verificationResult == PasswordVerificationResult.Failed)
        {
            throw new UnauthorizedException("Invalid username or password.");
        }

        if (verificationResult == PasswordVerificationResult.SuccessRehashNeeded)
        {
            user.PasswordHash = _passwordHasher.HashPassword(user, request.Password);
            await _context.SaveChangesAsync();
        }

        return CreateToken(user);
    }

    private AuthResponse CreateToken(AppUser user)
    {
        var expiresAtUtc = DateTime.UtcNow.AddMinutes(_jwtOptions.ExpirationMinutes);
        var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Secret));
        var credentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.UniqueName, user.Username),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var token = new JwtSecurityToken(
            issuer: _jwtOptions.Issuer,
            audience: _jwtOptions.Audience,
            claims: claims,
            expires: expiresAtUtc,
            signingCredentials: credentials);

        return new AuthResponse(
            new JwtSecurityTokenHandler().WriteToken(token),
            "Bearer",
            expiresAtUtc);
    }

    private static string NormalizeUsername(string username) =>
        username.Trim().ToUpperInvariant();
}
