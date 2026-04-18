using System.ComponentModel.DataAnnotations;

namespace FundManagement.Api.DTOs.Fund
{
    public class CreateFundDto
    {
        [Required]
        public string Name { get; set; } = null!;

        [Required]
        public string Category { get; set; } = null!;

        [Required]
        public decimal NAV { get; set; }
    }
}
