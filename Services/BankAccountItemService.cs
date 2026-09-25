using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
            .Where(p => p.Name.Equals(name))
            .Select(x => ItemToDTO(x))
            .ToListAsync();
    }

    public async Task<BankAccountItemDTO?> GetBankAccountItemById(int id)
    {
        var bankAccountItem = await _context.BankAccountItems.FindAsync(id);

        if (bankAccountItem == null)
        {
            return null;
        }

        return ItemToDTO(bankAccountItem);
    }

    public async Task<BankAccountItemDTO> CreateBankAccountItem(BankAccountItemDTO bankAccountDTO)
    {
        var bankAccountItem = new BankAccountItem
        {
            Name = bankAccountDTO.Name
        };

        _context.BankAccountItems.Add(bankAccountItem);
        await _context.SaveChangesAsync();

        return new BankAccountItemDTO
        {
            Id = bankAccountItem.Id,
            Name = bankAccountItem.Name
        };
    }

    public async Task<bool> UpdateBankAccountItem(int id, BankAccountItemDTO bankAccountDTO)
    {
        var bankAccountItem = await _context.BankAccountItems.FindAsync(id);
        if (bankAccountItem == null)
        {
            return false;
        }

        bankAccountItem.Name = bankAccountDTO.Name;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException) when (!BankAccountItemExists(id))
        {
            return false;
        }

        return true;            
    }

    public async Task<bool> DeleteBankAccountItem(int id)
    {
        var bankAccountItem = await _context.BankAccountItems.FindAsync(id);
        if (bankAccountItem == null)
        {
            return false;
        }

        _context.BankAccountItems.Remove(bankAccountItem);
        await _context.SaveChangesAsync();

        return true;
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
        };
}


