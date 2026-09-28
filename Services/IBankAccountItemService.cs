using BankAccountApi.Models;

namespace BankAccountApi.Services
{
    public interface IBankAccountItemService
    {
        Task<IEnumerable<BankAccountItemDTO>> GetAllBankAccountItems();
        Task<BankAccountItemDTO> GetBankAccountItemById(long id);
    }
}
