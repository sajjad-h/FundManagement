using FundManagement.Api.Data;
using FundManagement.Api.Models;

namespace FundManagement.Api.Data.Seed
{
    public class DataSeeder
    {
        public static async Task SeedAsync(AppDbContext context)
        {
            if (context.Funds.Any()) return; // prevent duplicate

            var funds = new List<Fund>
        {
            new Fund { Name = "Alpha Growth Fund", Category = "Equity", NAV = 120.50m, CreatedAt = DateTime.UtcNow },
            new Fund { Name = "Stable Income Fund", Category = "Debt", NAV = 98.20m, CreatedAt = DateTime.UtcNow },
            new Fund { Name = "Balanced Advantage Fund", Category = "Hybrid", NAV = 110.00m, CreatedAt = DateTime.UtcNow },
            new Fund { Name = "Equity Bluechip Fund", Category = "Equity", NAV = 150.75m, CreatedAt = DateTime.UtcNow },
            new Fund { Name = "Short Term Debt Fund", Category = "Debt", NAV = 101.10m, CreatedAt = DateTime.UtcNow }
        };

            var users = new List<User>
        {
            new User { Email = "admin@test.com", PasswordHash = "hashed", Role = "Admin" },
            new User { Email = "user@test.com", PasswordHash = "hashed", Role = "Investor" }
        };

            await context.Funds.AddRangeAsync(funds);
            await context.Users.AddRangeAsync(users);

            await context.SaveChangesAsync();
        }
    }
}
