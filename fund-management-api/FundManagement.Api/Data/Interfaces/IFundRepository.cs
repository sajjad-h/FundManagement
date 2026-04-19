using FundManagement.Api.Models;

namespace FundManagement.Api.Data.Interfaces
{
    public interface IFundRepository
    {
        Task<List<Fund>> GetAllAsync(string? category, bool? curNAVGreaterThan30FilterOn);
        Task<Fund?> GetByIdAsync(int id);
        Task<Fund> AddAsync(Fund fund);
    }
}
