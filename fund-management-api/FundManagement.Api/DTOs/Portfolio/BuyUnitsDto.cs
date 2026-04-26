namespace FundManagement.Api.DTOs.Portfolio
{
    public class BuyUnitsDto
    {
        public int UserId { get; set; }
        public int AccountId { get; set; }
        public int FundId { get; set; }
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
