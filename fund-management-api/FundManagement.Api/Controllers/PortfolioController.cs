using FundManagement.Api.DTOs.Portfolio;
using FundManagement.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FundManagement.Api.Controllers
{
    [ApiController]
    [Route("api/portfolios")]
    public class PortfolioController : ControllerBase
    {
        private readonly IPortfolioService _portfolioService;

        public PortfolioController(IPortfolioService portfolioService)
        {
            _portfolioService = portfolioService;
        }

        // POST /api/portfolios/buy
        [HttpPost("buy")]
        public async Task<IActionResult> BuyUnitsAsync(BuyUnitsDto buyUnitsDto)
        {
            await _portfolioService.BuyUnitsAsync(buyUnitsDto);
            return Ok();
        }

        // POST /api/portfolios/sell
        [HttpPost("sell")]
        public async Task<IActionResult> SellUnitsAsync(SellUnitsDto sellUnitsDto)
        {
            await _portfolioService.SellUnitsAsync(sellUnitsDto);
            return Ok();
        }

        // GET /api/portfolios/summary
        [HttpGet("summary")]
        public async Task<IActionResult> GetSummaryAsync([FromQuery] int userId)
        {
            var summary = await _portfolioService.GetSummaryAsync(userId);
            return Ok(summary);
        }
    }
}
