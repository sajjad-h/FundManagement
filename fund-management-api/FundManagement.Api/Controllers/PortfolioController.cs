using FundManagement.Api.Data.Interfaces;
using FundManagement.Api.Models;
using FundManagement.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace FundManagement.Api.Controllers
{
    [ApiController]
    [Route("api/portfolios")]
    public class PortfolioController : ControllerBase
    {
        private readonly PortfolioService _portfolioService;

        public PortfolioController(PortfolioService portfolioService)
        {
            _portfolioService = portfolioService;
        }

        // POST /api/portfolios/buy
        [HttpPost("buy")]
        public async Task<IActionResult> BuyUnitsAsync([FromQuery] int userId, [FromQuery] int accountId, [FromQuery] int fundId, [FromQuery] decimal amount)
        {
            await _portfolioService.BuyUnitsAsync(userId, accountId, fundId, amount);
            return Ok();
        }

        // POST /api/portfolios/sell
        [HttpPost("sell")]
        public async Task<IActionResult> SellUnitsAsync([FromQuery] int userId, [FromQuery] int accountId, [FromQuery] int fundId, [FromQuery] decimal unitsToSell)
        {
            await _portfolioService.SellUnitsAsync(userId, accountId, fundId, unitsToSell);
            return Ok();
        }
    }
}
