using FundManagement.Api.Models;

namespace FundManagement.Api.Models
{
    public class FundNAVHistory
    {
        public int Id { get; set; }
        public int FundId { get; set; }
        public decimal NAV { get; set; }
        public DateTime Date { get; set; }

        public Fund Fund { get; set; } = null!;
    }
}
