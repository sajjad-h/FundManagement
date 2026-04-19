using FundManagement.Api.Models;

namespace FundManagement.Api.Data.Interfaces
{
    public interface IFundNAVHistoryRepository
    {
        Task<List<FundNAVHistory>> GetByFundIdAsync(int fundId);
    }
}
