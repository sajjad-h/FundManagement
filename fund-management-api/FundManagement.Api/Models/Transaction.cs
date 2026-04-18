namespace FundManagement.Api.Models
{
    public class Transaction
    {
        public int Id { get; set; }
        public int PortfolioId { get; set; }
        public string Type { get; set; } = null!; // Buy / Sell
        public decimal Units { get; set; }
        public decimal NAV { get; set; }
        public DateTime Timestamp { get; set; }

        public Portfolio Portfolio { get; set; } = null!;
    }
}
