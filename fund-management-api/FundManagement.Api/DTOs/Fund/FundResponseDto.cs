namespace FundManagement.Api.DTOs.Fund
{
    public class FundResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Category { get; set; } = null!;
        public decimal NAV { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
