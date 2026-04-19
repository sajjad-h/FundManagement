using Microsoft.EntityFrameworkCore;

using FundManagement.Api.Data.Interfaces;
using FundManagement.Api.Models;

namespace FundManagement.Api.Data.Repositories
{
    public class FundRepository : IFundRepository
    {
        private readonly AppDbContext _context;

        public FundRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Fund>> GetAllAsync(string? category, bool? curNAVGreaterThan30FilterOn = false)
        {
            var query = _context.Funds
                .AsNoTracking()
                .AsQueryable();

            if (!string.IsNullOrEmpty(category))
                query = query.Where(f => f.Category == category);

            if (curNAVGreaterThan30FilterOn == true)
                query = query.Where(f =>
                    f.NAV >
                    (_context.FundNAVHistories
                        .Where(h => h.FundId == f.Id && h.Date >= DateTime.UtcNow.AddDays(-30))
                        .Average(h => (decimal?) h.NAV) ?? 0)
                );

            return await query.ToListAsync();
        }

        public async Task<Fund?> GetByIdAsync(int id)
        {
            return await _context.Funds
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<Fund> AddAsync(Fund fund)
        {
            _context.Funds.Add(fund);
            await _context.SaveChangesAsync();
            return fund;
        }
    }
}
