using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BankAccountApi.Models;
using BankAccountApi.Services;

namespace BankAccountApi.Controllers;

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
    [HttpGet("{id:int}")]
    public async Task<ActionResult<BankAccountItemDTO>> GetBankAccountItem(int id)
    {
        var bankAccountItem = await _bankAccountItemService.GetBankAccountItemById(id);
        if (bankAccountItem == null) return NotFound();

        return Ok(bankAccountItem);
    }
    
    // POST: api/BankAccountItems
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<BankAccountItemDTO>> PostBankAccountItem(BankAccountItemDTO bankAccountDTO)
    {
        var createdBankAccountItem = await _bankAccountItemService.CreateBankAccountItem(bankAccountDTO);
        return CreatedAtAction(
            nameof(GetBankAccountItem),
            new { id = createdBankAccountItem.Id },
            createdBankAccountItem);            
    }

    // PUT: api/BankAccountItems/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id:int}")]
    public async Task<IActionResult> PutBankAccountItem(int id, BankAccountItemDTO bankAccountDTO)
    {
        if (id != bankAccountDTO.Id)
        {
            return BadRequest();
        }
        
        if (!(await _bankAccountItemService.UpdateBankAccountItem(id, bankAccountDTO)))
        {
            return NotFound();
        }

        return NoContent();
    }


    // DELETE: api/BankAccountItems/5
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteBankAccountItem(int id)
    {
        if (!(await _bankAccountItemService.DeleteBankAccountItem(id)))
        {
            return NotFound();
        }

        return NoContent();
    }
}
