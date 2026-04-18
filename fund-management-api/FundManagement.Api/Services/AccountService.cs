using FundManagement.Api.Common.Exceptions;
using FundManagement.Api.Data.Interfaces;
using FundManagement.Api.DTOs.Account;
using FundManagement.Api.Models;
using FundManagement.Api.Services.Interfaces;

namespace FundManagement.Api.Services
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IUserRepository _userRepository;

        public AccountService(IAccountRepository accountRepository, IUserRepository userRepository)
        {
            _accountRepository = accountRepository;
            _userRepository = userRepository;
        }

        public async Task<List<AccountResponseDto>> GetAllAsync()
        {
            var accounts = await _accountRepository.GetAllAsync();
            if (accounts == null || !accounts.Any())
                return new List<AccountResponseDto>();

            return accounts.Select(account => new AccountResponseDto
            {
                Id = account.Id,
                UserId = account.UserId,
                Balance = account.Balance
            }).ToList();
        }

        public async Task<AccountResponseDto?> GetByIdAsync(int id)
        {
            var account = await _accountRepository.GetByIdAsync(id);
            if (account == null)
                throw new NotFoundException("Account not found");

            return new AccountResponseDto
            {
                Id = account.Id,
                UserId = account.UserId,
                Balance = account.Balance
            };
        }

        public async Task<AccountResponseDto> AddAsync(CreateAccountDto createAccountDto)
        {
            var user = await _userRepository.GetByIdAsync(createAccountDto.UserId);
            if (user == null)
                throw new NotFoundException("User not found");

            var account = new Account
            {
                UserId = createAccountDto.UserId,
                Balance = createAccountDto.InitialDeposit
            };

            await _accountRepository.AddAsync(account);

            return new AccountResponseDto
            {
                Id = account.Id,
                UserId = account.UserId,
                Balance = account.Balance
            };
        }
    }
}
