using FundManagement.Api.DTOs.Fund;

namespace FundManagement.Api.Services.Interfaces
{
    public interface IFundService
    {
        Task<List<FundResponseDto>> GetAllAsync(string? category);
        Task<FundResponseDto?> GetByIdAsync(int id);
        Task<FundResponseDto> AddAsync(CreateFundDto createFundDto);
    }
}
