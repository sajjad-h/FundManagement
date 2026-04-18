using Microsoft.EntityFrameworkCore;

using FundManagement.Api.Data.Interfaces;
using FundManagement.Api.Models;

namespace FundManagement.Api.Data.Repositories
{
    public class PortfolioRepository : IPortfolioRepository
    {
        private readonly AppDbContext _context;

        public PortfolioRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Portfolio>> GetAllAsync()
        {
            return await _context.Portfolios
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Portfolio?> GetByIdAsync(int id)
        {
            return await _context.Portfolios
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<Portfolio?> GetByUserIdAndFundIdAsync(int userId, int fundId)
        {
            return await _context.Portfolios
                .AsNoTracking()
                .Where(p => p.UserId == userId && p.FundId == fundId)
                .FirstOrDefaultAsync();
        }

        public async Task<Portfolio> AddAsync(Portfolio portfolio)
        {
            _context.Portfolios.Add(portfolio);
            await _context.SaveChangesAsync();
            return portfolio;
        }

        public async Task<Portfolio> UpdateAsync(Portfolio portfolio)
        {
            _context.Portfolios.Update(portfolio);
            await _context.SaveChangesAsync();
            return portfolio;
        }
    }
}
