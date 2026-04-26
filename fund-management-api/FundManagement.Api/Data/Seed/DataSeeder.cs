using FundManagement.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace FundManagement.Api.Data.Seed
{
    public static class DataSeeder
    {
        public static async Task SeedFundAndUserAsync(AppDbContext context)
        {
            if (!await context.Funds.AnyAsync())
            {
                var funds = new List<Fund>
                {
                    new Fund { Name = "Alpha Growth Fund", Category = "Equity", NAV = 120.50m, CreatedAt = DateTime.UtcNow },
                    new Fund { Name = "Stable Income Fund", Category = "Debt", NAV = 98.20m, CreatedAt = DateTime.UtcNow },
                    new Fund { Name = "Balanced Advantage Fund", Category = "Hybrid", NAV = 110.00m, CreatedAt = DateTime.UtcNow },
                    new Fund { Name = "Equity Bluechip Fund", Category = "Equity", NAV = 150.75m, CreatedAt = DateTime.UtcNow },
                    new Fund { Name = "Short Term Debt Fund", Category = "Debt", NAV = 101.10m, CreatedAt = DateTime.UtcNow }
                };

                await context.Funds.AddRangeAsync(funds);
                await context.SaveChangesAsync();
            }

            if (!await context.Users.AnyAsync())
            {
                var users = new List<User>
                {
                    new User
                    {
                        Email = "admin@test.com",
                        PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin@123"),
                        Role = Role.Admin
                    },
                    new User
                    {
                        Email = "investor@test.com",
                        PasswordHash = BCrypt.Net.BCrypt.HashPassword("Investor@123"),
                        Role = Role.Investor
                    }
                };

                await context.Users.AddRangeAsync(users);
                await context.SaveChangesAsync();
            }
        }

        public static async Task SeedNavHistoryAsync(AppDbContext context)
        {
            // Ensure DB is created
            await context.Database.EnsureCreatedAsync();

            // If already seeded, skip
            if (await context.FundNAVHistories.AnyAsync())
                return;

            var funds = await context.Funds.ToListAsync();

            if (!funds.Any())
                return;

            var random = new Random();
            var histories = new List<FundNAVHistory>();

            foreach (var fund in funds)
            {
                var baseNav = fund.NAV;

                for (int i = 0; i < 30; i++)
                {
                    var date = DateTime.UtcNow.Date.AddDays(-i);

                    // simulate NAV fluctuation ±5%
                    var fluctuation = (decimal)(random.NextDouble() * 0.1 - 0.05);
                    var nav = baseNav * (1 + fluctuation);

                    histories.Add(new FundNAVHistory
                    {
                        FundId = fund.Id,
                        Date = date,
                        NAV = Math.Round(nav, 4)
                    });
                }
            }

            await context.FundNAVHistories.AddRangeAsync(histories);
            await context.SaveChangesAsync();
        }
    }
}
