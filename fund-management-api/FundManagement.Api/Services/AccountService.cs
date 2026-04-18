using FundManagement.Api.Data;
using FundManagement.Api.Data.Interfaces;
using FundManagement.Api.Data.Repositories;
using FundManagement.Api.Models;

namespace FundManagement.Api.Services
{
    public class AccountService
    {
        private readonly IAccountRepository _accountRepository;

        public AccountService(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public async Task<List<Account>> GetAllAsync()
        {
            return await _accountRepository.GetAllAsync();
        }

        public async Task<Account?> GetByIdAsync(int id)
        {
            return await _accountRepository.GetByIdAsync(id);
        }

        public async Task<Account> AddAsync(Account account)
        {
            return await _accountRepository.AddAsync(account);
        }
    }
}
