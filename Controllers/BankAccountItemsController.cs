    using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BankAccountApi.Models;
using Microsoft.IdentityModel.Tokens;

namespace BankAccountApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BankAccountItemsController : ControllerBase
{
    private readonly BankAccountContext _context;

    public BankAccountItemsController(BankAccountContext context)
    {
        _context = context;
    }

    // GET: api/BankAccountItems
    [HttpGet]
    public async Task<ActionResult<IEnumerable<BankAccountItemDTO>>> GetBankAccountItems()
    {
        return await _context.BankAccountItems
            .Select(x => ItemToDTO(x))
            .ToListAsync();
    }

    [HttpGet("name/{name:alpha}")]
    public async Task<ActionResult<IEnumerable<BankAccountItemDTO>>> GetBankAccountByName(string name)
    {
        return await _context.BankAccountItems
            .Where(p => p.Name.Equals(name))
            .Select(x => ItemToDTO(x))
            .ToListAsync();
    }

    // GET: api/BankAccountItems/5
    // <snippet_GetByID>
    [HttpGet("{id:int}")]
    public async Task<ActionResult<BankAccountItemDTO>> GetBankAccountItem(int id)
    {
        var bankAccountItem = await _context.BankAccountItems.FindAsync(id);

        if (bankAccountItem == null)
        {
            return NotFound();
        }

        return ItemToDTO(bankAccountItem);
    }
    // </snippet_GetByID>p

    // PUT: api/BankAccountItems/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    // <snippet_Update>
    [HttpPut("{id:int}")]
    public async Task<IActionResult> PutBankAccountItem(int id, BankAccountItemDTO bankAccountDTO)
    {
        if (id != bankAccountDTO.Id)
        {
            return BadRequest();
        }

        var bankAccountItem = await _context.BankAccountItems.FindAsync(id);
        if (bankAccountItem == null)
        {
            return NotFound();
        }

        bankAccountItem.Name = bankAccountDTO.Name;
        bankAccountItem.Balance = bankAccountDTO.Balance;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException) when (!BankAccountItemExists(id))
        {
            return NotFound();
        }

        return NoContent();
    }
    // </snippet_Update>

    // POST: api/BankAccountItems
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    // <snippet_Create>
    [HttpPost]
    public async Task<ActionResult<BankAccountItemDTO>> PostBankAccountItem(BankAccountItemDTO bankAccountDTO)
    {
        var bankAccountItem = new BankAccountItem
        {
            Name = bankAccountDTO.Name,
            Balance = bankAccountDTO.Balance
        };

        _context.BankAccountItems.Add(bankAccountItem);
        await _context.SaveChangesAsync();

        return CreatedAtAction(
            nameof(GetBankAccountItem),
            new { id = bankAccountItem.Id },
            ItemToDTO(bankAccountItem));
    }
    // </snippet_Create>

    // DELETE: api/BankAccountItems/5
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteBankAccountItem(int id)
    {
        var bankAccountItem = await _context.BankAccountItems.FindAsync(id);
        if (bankAccountItem == null)
        {
            return NotFound();
        }

        _context.BankAccountItems.Remove(bankAccountItem);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool BankAccountItemExists(int id)
    {
        return _context.BankAccountItems.Any(e => e.Id == id);
    }

    private static BankAccountItemDTO ItemToDTO(BankAccountItem bankAccountItem) =>
       new BankAccountItemDTO
       {
           Id = bankAccountItem.Id,
           Name = bankAccountItem.Name,
           Balance = bankAccountItem.Balance,
       };
}
