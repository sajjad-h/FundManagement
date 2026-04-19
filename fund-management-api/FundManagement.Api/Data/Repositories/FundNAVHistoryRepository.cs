using Microsoft.EntityFrameworkCore;

using FundManagement.Api.Data.Interfaces;
using FundManagement.Api.Models;

namespace FundManagement.Api.Data.Repositories
{
    public class FundNAVHistoryRepository : IFundNAVHistoryRepository
    {
        private readonly AppDbContext _context;

        public FundNAVHistoryRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<FundNAVHistory>> GetByFundIdAsync(int fundId)
        {
            return await _context.FundNAVHistories
                .AsNoTracking()
                .Where(h => h.FundId == fundId)
                .OrderByDescending(h => h.Date)
                .ToListAsync();
        }
    }
}
