namespace FundManagement.Api.Models
{
    public class Portfolio
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int FundId { get; set; }
        public decimal Units { get; set; }
        public decimal PurchaseNAV { get; set; }

        public virtual User User { get; set; } = null!;
        public virtual Fund Fund { get; set; } = null!;
    }
}
