namespace FundManagement.Api.Models
{
    public class Fund
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Category { get; set; } = null!;
        public decimal NAV { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
