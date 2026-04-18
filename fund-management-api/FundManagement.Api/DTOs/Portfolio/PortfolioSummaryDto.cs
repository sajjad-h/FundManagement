namespace FundManagement.Api.DTOs.Portfolio
{
    public class PortfolioSummaryDto
    {
        public string Category { get; set; } = null!;
        public decimal TotalUnits { get; set; }
        public decimal CurrentValue { get; set; }
        public decimal Gain { get; set; }
    }
}
