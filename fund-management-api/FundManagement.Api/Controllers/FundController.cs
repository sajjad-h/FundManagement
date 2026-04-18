using FundManagement.Api.Data.Interfaces;
using FundManagement.Api.DTOs.Fund;
using FundManagement.Api.Models;
using FundManagement.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace FundManagement.Api.Controllers
{
    [ApiController]
    [Route("api/funds")]
    public class FundController : ControllerBase
    {
        private readonly FundService _fundService;

        public FundController(FundService fundService)
        {
            _fundService = fundService;
        }

        // GET /api/funds?category=Equity
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string? category)
        {
            var funds = await _fundService.GetAllAsync(category);
            return Ok(funds);
        }

        // GET /api/funds/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var fund = await _fundService.GetByIdAsync(id);
            return Ok(fund);
        }

        // POST /api/funds
        [HttpPost]
        public async Task<IActionResult> Create(CreateFundDto createFundDto)
        {
            // TODO: Admin check later
            var created = await _fundService.AddAsync(createFundDto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }
    }
}
