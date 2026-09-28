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

    // GET: api/BankAccountItems
    [HttpGet]
    public async Task<ActionResult<IEnumerable<BankAccountItemDTO>>> GetAllBankAccountItems()
    {
        return Ok(await _bankAccountItemService.GetAllBankAccountItems());
    }


    // GET: api/BankAccountItems/5
    [HttpGet("{id:long}")]
    public async Task<ActionResult<BankAccountItemDTO>> GetBankAccountItem(long id)
    {
        return Ok(await _bankAccountItemService.GetBankAccountItemById(id));
    }

}
