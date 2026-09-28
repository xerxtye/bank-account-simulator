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

    [HttpGet("name/{name:alpha}")]
    public async Task<ActionResult<IEnumerable<BankAccountItemDTO>>> GetBankAccountByName(string name)
    {
        return Ok(await _bankAccountItemService.GetBankAccountByName(name));
    }

    // GET: api/BankAccountItems/5
    [HttpGet("{id:long}")]
    public async Task<ActionResult<BankAccountItemDTO>> GetBankAccountItem(long id)
    {
        return Ok(await _bankAccountItemService.GetBankAccountItemById(id));
    }

    // POST: api/BankAccountItems
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<BankAccountItemDTO>> PostBankAccountItem(BankAccountItemDTO bankAccountDTO)
    {
        return Ok(await _bankAccountItemService.CreateBankAccountItem(bankAccountDTO));
    }

    // PUT: api/BankAccountItems/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id:long}")]
    public async Task<IActionResult> PutBankAccountItem(long id, BankAccountItemDTO bankAccountDTO)
    {
        await _bankAccountItemService.UpdateBankAccountItem(id, bankAccountDTO);
        return Ok();
    }


    // DELETE: api/BankAccountItems/5
    [HttpDelete("{id:long}")]
    public async Task<IActionResult> DeleteBankAccountItem(long id)
    {
        await _bankAccountItemService.DeleteBankAccountItem(id);
        return Ok();
    }
}
