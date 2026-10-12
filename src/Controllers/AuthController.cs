using BankAccountApi.Models;
using BankAccountApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BankAccountApi.Controllers;

[ApiController]
[AllowAnonymous]
[Route("api/auth")]
public sealed class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    /// <summary>Register a new bank account.</summary>
    /// <remarks>Creates an account, securely hashes its password, and returns the generated account ID.</remarks>
    /// <param name="request">The account owner's name and password.</param>
    /// <response code="200">The account was created.</response>
    /// <response code="400">The request data is invalid.</response>
    [HttpPost("register")]
    [ProducesResponseType<RegisteredUserResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<RegisteredUserResponse>> Register(RegisterRequest request)
    {
        return Ok(await _authService.Register(request));
    }

    /// <summary>Log in to a bank account.</summary>
    /// <remarks>Validates the account ID and password, then returns a JWT access token.</remarks>
    /// <param name="request">The generated account ID and account password.</param>
    /// <response code="200">Authentication succeeded and a JWT was issued.</response>
    /// <response code="401">The account ID or password is invalid.</response>
    [HttpPost("login")]
    [ProducesResponseType<AuthResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<AuthResponse>> Login(LoginRequest request)
    {
        return Ok(await _authService.Login(request));
    }
}
