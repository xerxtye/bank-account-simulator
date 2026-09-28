using BankAccountApi.Models;

namespace BankAccountApi.Services
{
    public interface IBankAccountItemService
    {
        Task<IEnumerable<BankAccountItemDTO>> GetAllBankAccountItems();
        Task<BankAccountItemDTO> GetBankAccountItemById(long id);
        Task<IEnumerable<BankAccountItemDTO>> GetBankAccountByName(string name);
        Task<BankAccountItemDTO> CreateBankAccountItem(BankAccountItemDTO bankAccountItemDTO);
        Task UpdateBankAccountItem(long id, BankAccountItemDTO bankAccountItemDTO);
        Task DeleteBankAccountItem(long id);
    }
}
