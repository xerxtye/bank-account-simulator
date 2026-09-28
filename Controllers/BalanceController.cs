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

    [HttpGet]
    public async Task<ActionResult<BalanceResponse>> GetBalance()
    {
        return Ok(await _balanceService.GetBalance());
    }

    [HttpPost("deposit")]
    public async Task<ActionResult<BalanceOperationResponse>> Deposit(MoneyRequest request)
    {
        return Ok(await _balanceService.Deposit(request));
    }

    [HttpPost("withdraw")]
    public async Task<ActionResult<BalanceOperationResponse>> Withdraw(MoneyRequest request)
    {
        return Ok(await _balanceService.Withdraw(request));
    }
}
