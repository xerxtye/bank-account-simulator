using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BankAccountApi.Models;
using BankAccountApi.Services;

namespace BankAccountApi.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class BankAccountItemsController : ControllerBase
{
    private readonly IBankAccountItemService _bankAccountItemService;

    public BankAccountItemsController(IBankAccountItemService bankAccountItemService)
    {
        _bankAccountItemService = bankAccountItemService;
    }

    /// <summary>List bank accounts.</summary>
    /// <remarks>Returns public account data only. Password hashes and balances are never included.</remarks>
    /// <response code="200">The public account list.</response>
    /// <response code="401">A valid JWT is required.</response>
    [HttpGet]
    [ProducesResponseType<IEnumerable<BankAccountItemDTO>>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<IEnumerable<BankAccountItemDTO>>> GetAllBankAccountItems()
    {
        return Ok(await _bankAccountItemService.GetAllBankAccountItems());
    }


    /// <summary>Get public information about a bank account.</summary>
    /// <param name="id">The generated bank account ID.</param>
    /// <response code="200">The public account data.</response>
    /// <response code="404">The account was not found.</response>
    [HttpGet("{id:long}")]
    [ProducesResponseType<BankAccountItemDTO>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<BankAccountItemDTO>> GetBankAccountItem(long id)
    {
        return Ok(await _bankAccountItemService.GetBankAccountItemById(id));
    }

}
