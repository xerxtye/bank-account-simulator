using BankAccountApi.Models;
using BankAccountApi.Security.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace BankAccountApi.Services;

public sealed class BankAccountItemService : IBankAccountItemService
{
    private readonly BankAccountContext _context;

    public BankAccountItemService(BankAccountContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<BankAccountItemDTO>> GetAllBankAccountItems()
    {
        return await _context.BankAccountItems
            .Select(account => ItemToDTO(account))
            .ToListAsync();
    }


    public async Task<BankAccountItemDTO> GetBankAccountItemById(long id)
    {
        var bankAccountItem = await _context.BankAccountItems.FindAsync(id)
            ?? throw new NotFoundException($"Bank account with ID {id} was not found.");

        return ItemToDTO(bankAccountItem);
    }

    private static BankAccountItemDTO ItemToDTO(BankAccountItem bankAccountItem) =>
        new()
        {
            Id = bankAccountItem.Id,
            FirstName = bankAccountItem.FirstName,
            LastName = bankAccountItem.LastName
        };
}
