using FundManagement.Api.Models;

namespace FundManagement.Api.Data.Interfaces
{
    public interface IPortfolioRepository
    {
        Task<List<Portfolio>> GetAllAsync();
        Task<Portfolio?> GetByIdAsync(int id);
        Task<Portfolio?> GetByUserIdAndFundIdAsync(int userId, int fundId);
        Task<Portfolio> AddAsync(Portfolio portfolio);
        Task<Portfolio> UpdateAsync(Portfolio portfolio);
    }
}
