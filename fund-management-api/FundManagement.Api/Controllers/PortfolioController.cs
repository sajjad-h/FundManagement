using FundManagement.Api.Data.Interfaces;
using FundManagement.Api.DTOs.Portfolio;
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
    }
}
