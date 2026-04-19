using FundManagement.Api.DTOs.Fund;
using FundManagement.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FundManagement.Api.Controllers
{
    [ApiController]
    [Route("api/funds")]
    public class FundController : ControllerBase
    {
        private readonly IFundService _fundService;

        public FundController(IFundService fundService)
        {
            _fundService = fundService;
        }

        // GET /api/funds?category=Equity
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string? category, [FromQuery] bool? curNAVGreaterThan30FilterOn)
        {
            var funds = await _fundService.GetAllAsync(category, curNAVGreaterThan30FilterOn);
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

        // GET /api/funds/{id}
        [HttpGet("{id}/nav-history")]
        public async Task<IActionResult> GetFundNAVHistoryByFundId(int id)
        {
            var histories = await _fundService.GetFundNAVHistoryByFundIdAsync(id);
            return Ok(histories);
        }
    }
}
