using BankAccountApi.Models;

namespace BankAccountApi.Services;

public interface IAuthService
{
    Task<RegisteredUserResponse> Register(RegisterRequest request);
    Task<AuthResponse> Login(LoginRequest request);
}
