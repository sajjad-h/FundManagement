namespace FundManagement.Api.DTOs.Fund
{
    public class NAVUpdateDto
    {
        public int FundId { get; set; }
        public decimal NAV { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
