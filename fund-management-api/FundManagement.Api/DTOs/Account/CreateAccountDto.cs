using System.ComponentModel.DataAnnotations;

namespace FundManagement.Api.DTOs.Account
{
    public class CreateAccountDto
    {
        [Required]
        public int UserId { get; set; }

        [Required]
        public decimal InitialDeposit { get; set; }
    }
}
