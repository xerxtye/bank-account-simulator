using Microsoft.EntityFrameworkCore;
using BankAccountApi.Security.Exceptions;
using BankAccountApi.Models;

namespace BankAccountApi.Services;

public class BankAccountItemService : IBankAccountItemService
{
    private readonly BankAccountContext _context;

    public BankAccountItemService(BankAccountContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<BankAccountItemDTO>> GetAllBankAccountItems()
    {
        return await _context.BankAccountItems
            .Select(x => ItemToDTO(x))
            .ToListAsync();
    }

    public async Task<IEnumerable<BankAccountItemDTO>> GetBankAccountByName(string name)
    {
        return await _context.BankAccountItems
            .Where(p => p.Name == name)
            .Select(x => ItemToDTO(x))
            .ToListAsync();
    }

    public async Task<BankAccountItemDTO> GetBankAccountItemById(int id)
    {
        var bankAccountItem = await _context.BankAccountItems.FindAsync(id)
            ?? throw new NotFoundException($"Bank account item with ID {id} was not found.");

        return ItemToDTO(bankAccountItem);
    }

    public async Task<BankAccountItemDTO> CreateBankAccountItem(BankAccountItemDTO bankAccountDTO)
    {
        var bankAccountItem = new BankAccountItem
        {
            Name = bankAccountDTO.Name,
            Balance = bankAccountDTO.Balance
        };

        _context.BankAccountItems.Add(bankAccountItem);
        await _context.SaveChangesAsync();

        return ItemToDTO(bankAccountItem);
    }

    public async Task UpdateBankAccountItem(int id, BankAccountItemDTO bankAccountDTO)
    {
        if (id != bankAccountDTO.Id)
        {
            throw new BadRequestException("The route ID must match the request body ID.");
        }

        var bankAccountItem = await _context.BankAccountItems.FindAsync(id)
            ?? throw new NotFoundException($"Bank account item with ID {id} was not found.");

        bankAccountItem.Name = bankAccountDTO.Name;
        bankAccountItem.Balance = bankAccountDTO.Balance;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException) when (!BankAccountItemExists(id))
        {
            throw new NotFoundException($"Bank account item with ID {id} was not found.");
        }
    }

    public async Task DeleteBankAccountItem(int id)
    {
        var bankAccountItem = await _context.BankAccountItems.FindAsync(id)
            ?? throw new NotFoundException($"Bank account item with ID {id} was not found.");

        _context.BankAccountItems.Remove(bankAccountItem);
        await _context.SaveChangesAsync();
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
            Balance = bankAccountItem.Balance
        };
}
