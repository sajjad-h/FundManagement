using FundManagement.Api.Models;

namespace FundManagement.Api.Models
{
    public class Portfolio
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int FundId { get; set; }
        public decimal Units { get; set; }
        public decimal PurchaseNAV { get; set; }

        public User User { get; set; } = null!;
        public Fund Fund { get; set; } = null!;
    }
}
