using FundManagement.Api.Models;

namespace FundManagement.Api.Data.Interfaces
{
    public interface IAccountRepository
    {
        Task<List<Account>> GetAllAsync();
        Task<Account?> GetByIdAsync(int id);
        Task<Account?> GetByIdAndUserIdAsync(int id, int userId);
        Task<Account> AddAsync(Account account);
        Task<Account> UpdateAsync(Account account);
    }
}
