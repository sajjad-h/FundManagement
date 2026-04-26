using FundManagement.Api.DTOs.Fund;

namespace FundManagement.Api.Services.Interfaces
{
    public interface IFundService
    {
        Task<List<FundResponseDto>> GetAllAsync(string? category, bool? curNAVGreaterThan30FilterOn);
        Task<FundResponseDto?> GetByIdAsync(int id);
        Task<FundResponseDto> AddAsync(CreateFundDto createFundDto);
        Task<FundResponseDto> UpdateAsync(int id, UpdateFundDto dto);
        Task DeleteAsync(int id);
        Task<List<NAVRecordDto>> GetFundNAVHistoryByFundIdAsync(int fundId);
        IAsyncEnumerable<NAVRecordDto> GetStreamFundNAVHistoryByFundIdAsync(int fundId);
    }
}
