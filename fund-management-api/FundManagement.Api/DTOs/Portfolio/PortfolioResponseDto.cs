namespace FundManagement.Api.DTOs.Portfolio
{
    public class PortfolioResponseDto
    {
        public int Id { get; set; }
        public string UserEmail { get; set; } = null!;
        public string FundName { get; set; } = null!;
        public string FundCategory { get; set; } = null!;
        public decimal Units { get; set; }
        public decimal PurchaseNAV { get; set; }
    }
}
