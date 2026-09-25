using Microsoft.AspNetCore.Mvc;
using BankAccountApi.Models;

namespace BankAccountApi.Services
{
    public interface IBankAccountItemService
    {
        Task<IEnumerable<BankAccountItemDTO>> GetAllBankAccountItems();
        Task<BankAccountItemDTO?> GetBankAccountItemById(int id);
        Task<IEnumerable<BankAccountItemDTO>> GetBankAccountByName(string name);
        Task<BankAccountItemDTO> CreateBankAccountItem(BankAccountItemDTO bankAccountItemDTO);
        Task<bool> UpdateBankAccountItem(int id, BankAccountItemDTO bankAccountItemDTO);
        Task<bool> DeleteBankAccountItem(int id);
    }
}
