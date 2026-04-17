using FundManagement.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace FundManagement.Api.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) { }

        public DbSet<Fund> Funds { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Portfolio> Portfolios { get; set; }
        public DbSet<Transaction> Transactions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Fund>()
                .Property(f => f.NAV)
                .HasPrecision(18, 4);

            modelBuilder.Entity<Portfolio>()
                .HasOne(p => p.User)
                .WithMany()
                .HasForeignKey(p => p.UserId);

            modelBuilder.Entity<Portfolio>()
                .HasOne(p => p.Fund)
                .WithMany()
                .HasForeignKey(p => p.FundId);

            modelBuilder.Entity<Transaction>()
                .HasOne(t => t.Portfolio)
                .WithMany()
                .HasForeignKey(t => t.PortfolioId);
        }
    }
}
