using Microsoft.EntityFrameworkCore;

using FundManagement.Api.Data.Interfaces;
using FundManagement.Api.Models;
using FundManagement.Api.DTOs.Portfolio;

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
            var portfolios = await _context.Portfolios
                // This line makes eager loading, without this lazy loading when enabled
                //.Include(p => p.Fund)
                .ToListAsync();

            // This part doesn't make duplicate queries as tracking is active. But service side there are no tracking so duplicate query exists.
            var result = portfolios.Select(p => new PortfolioResponseDto
            {
                Id = p.Id,
                UserEmail = p.User.Email,
                FundName = p.Fund.Name,   // 🔥 triggers query per row
                FundCategory = p.Fund.Category,
                Units = p.Units,
                PurchaseNAV = p.PurchaseNAV
            }).ToList();

            // No tracking here. Duplicate query exists. EF Core tracking is very smart.
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

        public async Task<List<PortfolioSummaryDto>> GetSummaryAsync(int userId)
        {
            var summary = await _context.Portfolios
                .AsNoTracking()
                .Where(p => p.UserId == userId)
                .Select(p => new
                {
                    p.Units,
                    p.Fund.Category,
                    CurrentNAV = p.Fund.NAV,
                    p.PurchaseNAV
                })
                .GroupBy(x => x.Category)
                .Select(g => new PortfolioSummaryDto
                {
                    Category = g.Key,
                    TotalUnits = g.Sum(x => x.Units),
                    CurrentValue = g.Sum(x => x.Units * x.CurrentNAV),
                    Gain = g.Sum(x => x.Units * (x.CurrentNAV - x.PurchaseNAV))
                })
                .ToListAsync();
            return summary;
        }
    }
}
