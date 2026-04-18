using FundManagement.Api.Models;
using System.ComponentModel.DataAnnotations;

namespace FundManagement.Api.DTOs.Portfolio
{
    public class SellUnitsDto
    {
        [Required]
        public int UserId { get; set; }

        [Required]
        public int AccountId { get; set; }

        [Required]
        public int FundId { get; set; }

        [Required]
        public decimal UnitsToSell { get; set; }

        public void Deconstruct(out int userId, out int accountId, out int fundId, out decimal unitsToSell)
        {
            userId = UserId;
            accountId = AccountId;
            fundId = FundId;
            unitsToSell = UnitsToSell;
        }
    }
}
