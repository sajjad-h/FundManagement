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

        public async Task<List<Fund>> GetAllAsync(string? category)
        {
            var query = _context.Funds.AsQueryable();

            if (!string.IsNullOrEmpty(category))
                query = query.Where(f => f.Category == category);

            return await query.ToListAsync();
        }

        public async Task<Fund?> GetByIdAsync(int id)
        {
            return await _context.Funds.FindAsync(id);
        }

        public async Task<Fund> AddAsync(Fund fund)
        {
            _context.Funds.Add(fund);
            await _context.SaveChangesAsync();
            return fund;
        }
    }
}
