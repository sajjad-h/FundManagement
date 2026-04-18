using FundManagement.Api.DTOs.Account;

namespace FundManagement.Api.Services.Interfaces
{
    public interface IAccountService
    {
        Task<List<AccountResponseDto>> GetAllAsync();
        Task<AccountResponseDto?> GetByIdAsync(int id);
        Task<AccountResponseDto> AddAsync(CreateAccountDto createResponseDto);
    }
}
