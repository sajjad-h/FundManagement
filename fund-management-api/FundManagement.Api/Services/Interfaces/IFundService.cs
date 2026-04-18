using FundManagement.Api.DTOs.Account;
using FundManagement.Api.DTOs.Fund;
using FundManagement.Api.Models;

namespace FundManagement.Api.Services.Interfaces
{
    public interface IFundService
    {
        Task<List<FundResponseDto>> GetAllAsync(string? category);
        Task<FundResponseDto?> GetByIdAsync(int id);
        Task<FundResponseDto> AddAsync(CreateFundDto createFundDto);
    }
}
