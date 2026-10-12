using BankAccountApi.Models;
using BankAccountApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BankAccountApi.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public sealed class BalanceController : ControllerBase
{
    private readonly IBalanceService _balanceService;

    public BalanceController(IBalanceService balanceService)
    {
        _balanceService = balanceService;
    }

    /// <summary>Get the current account balance.</summary>
    /// <remarks>Returns the balance of the account identified by the JWT.</remarks>
    /// <response code="200">The current balance.</response>
    /// <response code="401">A valid JWT is required.</response>
    [HttpGet]
    [ProducesResponseType<BalanceResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<BalanceResponse>> GetBalance()
    {
        return Ok(await _balanceService.GetBalance());
    }

    /// <summary>Deposit money into the current account.</summary>
    /// <param name="request">A positive monetary amount to deposit.</param>
    /// <response code="200">The deposit succeeded and the updated balance is returned.</response>
    /// <response code="400">The amount is invalid.</response>
    /// <response code="409">The maximum account balance would be exceeded.</response>
    [HttpPost("deposit")]
    [ProducesResponseType<BalanceOperationResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<BalanceOperationResponse>> Deposit(MoneyRequest request)
    {
        return Ok(await _balanceService.Deposit(request));
    }

    /// <summary>Withdraw money from the current account.</summary>
    /// <param name="request">A positive monetary amount to withdraw.</param>
    /// <response code="200">The withdrawal succeeded and the updated balance is returned.</response>
    /// <response code="400">The amount is invalid.</response>
    /// <response code="409">The account has insufficient funds.</response>
    [HttpPost("withdraw")]
    [ProducesResponseType<BalanceOperationResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<BalanceOperationResponse>> Withdraw(MoneyRequest request)
    {
        return Ok(await _balanceService.Withdraw(request));
    }
}
