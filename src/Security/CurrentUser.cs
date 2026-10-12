using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using BankAccountApi.Security.Exceptions;

namespace BankAccountApi.Security;

public interface ICurrentUser
{
    long Id { get; }
}

public sealed class CurrentUser : ICurrentUser
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUser(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public long Id
    {
        get
        {
            var principal = _httpContextAccessor.HttpContext?.User;
            var subject = principal?.FindFirstValue(ClaimTypes.NameIdentifier)
                ?? principal?.FindFirstValue(JwtRegisteredClaimNames.Sub);

            if (!long.TryParse(subject, out var userId))
            {
                throw new UnauthorizedException("The access token does not identify a valid user.");
            }

            return userId;
        }
    }
}
