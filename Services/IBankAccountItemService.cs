using BankAccountApi.Models;

namespace BankAccountApi.Services
{
    public interface IBankAccountItemService
    {
        Task<IEnumerable<BankAccountItemDTO>> GetAllBankAccountItems();
        Task<BankAccountItemDTO> GetBankAccountItemById(int id);
        Task<IEnumerable<BankAccountItemDTO>> GetBankAccountByName(string name);
        Task<BankAccountItemDTO> CreateBankAccountItem(BankAccountItemDTO bankAccountItemDTO);
        Task UpdateBankAccountItem(int id, BankAccountItemDTO bankAccountItemDTO);
        Task DeleteBankAccountItem(int id);
    }
}
