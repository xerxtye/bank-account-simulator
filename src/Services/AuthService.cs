using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BankAccountApi.Models;
using BankAccountApi.Security;
using BankAccountApi.Security.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace BankAccountApi.Services;

public sealed class AuthService : IAuthService
{
    private readonly BankAccountContext _context;
    private readonly IPasswordHasher<BankAccountItem> _passwordHasher;
    private readonly JwtOptions _jwtOptions;

    public AuthService(
        BankAccountContext context,
        IPasswordHasher<BankAccountItem> passwordHasher,
        IOptions<JwtOptions> jwtOptions)
    {
        _context = context;
        _passwordHasher = passwordHasher;
        _jwtOptions = jwtOptions.Value;
    }

    public async Task<RegisteredUserResponse> Register(RegisterRequest request)
    {
        var firstName = request.FirstName.Trim();
        var lastName = request.LastName.Trim();
        if (firstName.Length < 2 || lastName.Length < 2)
        {
            throw new BadRequestException(
                "First name and last name must contain at least 2 non-whitespace characters.");
        }


        var user = new BankAccountItem
        {
            FirstName = firstName,
            LastName = lastName,
            PasswordHash = string.Empty
        };

        user.PasswordHash = _passwordHasher.HashPassword(user, request.Password);

        _context.BankAccountItems.Add(user);
        await _context.SaveChangesAsync();

        return new RegisteredUserResponse(
            user.Id,
            user.FirstName,
            user.LastName);
    }

    public async Task<AuthResponse> Login(LoginRequest request)
    {
        var user = await _context.BankAccountItems.FindAsync(request.AccountId);

        if (user is null)
        {
            throw new UnauthorizedException("Invalid account ID or password.");
        }

        var verificationResult = _passwordHasher.VerifyHashedPassword(
            user,
            user.PasswordHash,
            request.Password);

        if (verificationResult == PasswordVerificationResult.Failed)
        {
            throw new UnauthorizedException("Invalid account ID or password.");
        }

        if (verificationResult == PasswordVerificationResult.SuccessRehashNeeded)
        {
            user.PasswordHash = _passwordHasher.HashPassword(user, request.Password);
            await _context.SaveChangesAsync();
        }

        return CreateToken(user);
    }

    private AuthResponse CreateToken(BankAccountItem user)
    {
        var expiresAtUtc = DateTime.UtcNow.AddMinutes(_jwtOptions.ExpirationMinutes);
        var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Secret));
        var credentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
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

}
