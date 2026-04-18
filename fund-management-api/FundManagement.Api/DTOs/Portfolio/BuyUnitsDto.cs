using System.ComponentModel.DataAnnotations;

namespace FundManagement.Api.DTOs.Portfolio
{
    public class BuyUnitsDto
    {
        [Required]
        public int UserId { get; set; }

        [Required]
        public int AccountId { get; set; }

        [Required]
        public int FundId { get; set; }

        [Required]
        public decimal Amount { get; set; }

        public void Deconstruct(out int userId, out int accountId, out int fundId, out decimal amount)
        {
            userId = UserId;
            accountId = AccountId;
            fundId = FundId;
            amount = Amount;
        }
    }
}
