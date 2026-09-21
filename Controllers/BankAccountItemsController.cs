using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BankAccountApi.Models;

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

    // // GET: api/test
    // [HttpGet]
    // public async Task<ActionResult<IEnumerable<BankAccountItemDTO>>> GetTest()
    // {
    //     return await _context.BankAccountItems
    //         .Select(x => ItemToDTO(x))
    //         .ToListAsync();
    // }

    
    // GET: api/BankAccountItems/5
    // <snippet_GetByID>
    [HttpGet("{id}")]
    public async Task<ActionResult<BankAccountItemDTO>> GetBankAccountItem(long id)
    {
        var todoItem = await _context.BankAccountItems.FindAsync(id);

        if (todoItem == null)
        {
            return NotFound();
        }

        return ItemToDTO(todoItem);
    }
    // </snippet_GetByID>p

    // PUT: api/BankAccountItems/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    // <snippet_Update>
    [HttpPut("{id}")]
    public async Task<IActionResult> PutBankAccountItem(long id, BankAccountItemDTO todoDTO)
    {
        if (id != todoDTO.Id)
        {
            return BadRequest();
        }

        var todoItem = await _context.BankAccountItems.FindAsync(id);
        if (todoItem == null)
        {
            return NotFound();
        }

        todoItem.Name = todoDTO.Name;
        todoItem.IsComplete = todoDTO.IsComplete;

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
    public async Task<ActionResult<BankAccountItemDTO>> PostBankAccountItem(BankAccountItemDTO todoDTO)
    {
        var todoItem = new BankAccountItem
        {
            IsComplete = todoDTO.IsComplete,
            Name = todoDTO.Name
        };

        _context.BankAccountItems.Add(todoItem);
        await _context.SaveChangesAsync();

        return CreatedAtAction(
            nameof(GetBankAccountItem),
            new { id = todoItem.Id },
            ItemToDTO(todoItem));
    }
    // </snippet_Create>

    // DELETE: api/BankAccountItems/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteBankAccountItem(long id)
    {
        var todoItem = await _context.BankAccountItems.FindAsync(id);
        if (todoItem == null)
        {
            return NotFound();
        }

        _context.BankAccountItems.Remove(todoItem);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool BankAccountItemExists(long id)
    {
        return _context.BankAccountItems.Any(e => e.Id == id);
    }

    private static BankAccountItemDTO ItemToDTO(BankAccountItem todoItem) =>
       new BankAccountItemDTO
       {
           Id = todoItem.Id,
           Name = todoItem.Name,
           IsComplete = todoItem.IsComplete
       };
}
