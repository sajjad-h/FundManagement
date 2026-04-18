using FundManagement.Api.Common.Exceptions;
using FundManagement.Api.Data;
using FundManagement.Api.Data.Interfaces;
using FundManagement.Api.Data.Repositories;
using FundManagement.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace FundManagement.Api.Services
{
    public class PortfolioService
    {
        private readonly IUserRepository _userRepository;
        private readonly IFundRepository _fundRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly IPortfolioRepository _portfolioRepository;
        private readonly ITransactionRepository _transactionRepository;

        public PortfolioService(IUserRepository userRepository, IFundRepository fundRepository, IAccountRepository accountRepository, IPortfolioRepository portfolioRepository, ITransactionRepository transactionRepository)
        {
            _userRepository = userRepository;
            _fundRepository = fundRepository;
            _accountRepository = accountRepository;
            _portfolioRepository = portfolioRepository;
            _transactionRepository = transactionRepository;
        }

        public async Task BuyUnitsAsync(int userId, int accountId, int fundId, decimal amount)
        {
            try
            {
                var user = await _userRepository.GetByIdAsync(userId);
                if (user == null)
                    throw new NotFoundException("User not found");

                var account = await _accountRepository.GetByIdAndUserIdAsync(accountId, userId);
                if (account == null)
                    throw new NotFoundException("Account not found");

                var fund = await _fundRepository.GetByIdAsync(fundId);
                if (fund == null)
                    throw new NotFoundException("Fund not found");

                if (account.Balance < amount)
                    throw new BadRequestException("Insufficient balance");

                var units = amount / fund.NAV;

                // Debit balance
                account.Balance -= amount;

                // Get or create portfolio
                var portfolio = await _portfolioRepository.GetByUserIdAndFundIdAsync(userId, fundId);
                if (portfolio == null)
                {
                    portfolio = new Portfolio
                    {
                        UserId = userId,
                        FundId = fundId,
                        Units = 0,
                        PurchaseNAV = fund.NAV
                    };
                    portfolio = await _portfolioRepository.AddAsync(portfolio);
                }

                portfolio.Units += units;

                await _portfolioRepository.UpdateAsync(portfolio);

                // Log transaction
                var transaction = new Transaction
                {
                    Portfolio = portfolio,
                    Type = "Buy",
                    Units = units,
                    NAV = fund.NAV,
                    Timestamp = DateTime.UtcNow
                };

                await _transactionRepository.AddAsync(transaction);
            }
            catch
            {
                throw;
            }
        }

        public async Task SellUnitsAsync(int userId, int accountId, int fundId, decimal unitsToSell)
        {
            try
            {
                var user = await _userRepository.GetByIdAsync(userId);
                if (user == null)
                    throw new NotFoundException("User not found");

                var account = await _accountRepository.GetByIdAndUserIdAsync(accountId, userId);
                if (account == null)
                    throw new NotFoundException("Account not found");

                var fund = await _fundRepository.GetByIdAsync(fundId);
                if (fund == null)
                    throw new NotFoundException("Fund not found");

                var portfolio = await _portfolioRepository.GetByUserIdAndFundIdAsync(userId, fundId);
                if (portfolio == null)
                    throw new NotFoundException("Portfolio not found");

                if (portfolio.Units < unitsToSell)
                    throw new BadRequestException("Not enough units");

                var amount = unitsToSell * fund.NAV;

                // Reduce units
                portfolio.Units -= unitsToSell;
                await _portfolioRepository.UpdateAsync(portfolio);

                // Credit balance
                account.Balance += amount;
                await _accountRepository.UpdateAsync(account);

                // Log transaction
                var transaction = new Transaction
                {
                    PortfolioId = portfolio.Id,
                    Type = "Sell",
                    Units = unitsToSell,
                    NAV = fund.NAV,
                    Timestamp = DateTime.UtcNow
                };

                await _transactionRepository.AddAsync(transaction);
            }
            catch
            {
                throw;
            }
        }
    }
}
