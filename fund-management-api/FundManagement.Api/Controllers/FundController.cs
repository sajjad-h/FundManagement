using FundManagement.Api.Data.Interfaces;
using FundManagement.Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace FundManagement.Api.Controllers
{
    [ApiController]
    [Route("api/funds")]
    public class FundController : ControllerBase
    {
        private readonly IFundRepository _repo;

        public FundController(IFundRepository repo)
        {
            _repo = repo;
        }

        // GET /api/funds?category=Equity
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string? category)
        {
            var funds = await _repo.GetAllAsync(category ?? "");
            return Ok(funds);
        }

        // GET /api/funds/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var fund = await _repo.GetByIdAsync(id);
            if (fund == null) return NotFound();
            return Ok(fund);
        }

        // POST /api/funds
        [HttpPost]
        public async Task<IActionResult> Create(Fund fund)
        {
            // TODO: Admin check later
            var created = await _repo.AddAsync(fund);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }
    }
}
