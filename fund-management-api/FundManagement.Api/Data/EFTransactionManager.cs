using FundManagement.Api.Services.Interfaces;

namespace FundManagement.Api.Data
{
    public class EFTransactionManager : ITransactionManager
    {
        private readonly AppDbContext _context;

        public EFTransactionManager(AppDbContext context)
        {
            _context = context;
        }

        public async Task ExecuteAsync(Func<Task> action)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                await action();

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}
