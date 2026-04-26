namespace FundManagement.Api.DTOs.Fund
{
    public class UpdateFundDto
    {
        public string Name { get; set; } = null!;
        public string Category { get; set; } = null!;
        public decimal NAV { get; set; }
    }
}
