using FundManagement.Api.DTOs.Fund;
using FundManagement.Api.Models;
using FundManagement.Api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FundManagement.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/funds")]
    public class FundController : ControllerBase
    {
        private readonly IFundService _fundService;

        public FundController(IFundService fundService)
        {
            _fundService = fundService;
        }

        // TODO: Maybe it should be a public API
        // GET /api/funds?category=Equity
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string? category, [FromQuery] bool? curNAVGreaterThan30FilterOn)
        {
            var funds = await _fundService.GetAllAsync(category, curNAVGreaterThan30FilterOn);
            return Ok(funds);
        }

        // TODO: Maybe it should be a public API
        // GET /api/funds/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var fund = await _fundService.GetByIdAsync(id);
            return Ok(fund);
        }

        // POST /api/funds
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Create(CreateFundDto createFundDto)
        {
            var created = await _fundService.AddAsync(createFundDto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        // PUT /api/funds/{id}
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateFundDto updateFundDto)
        {
            var updated = await _fundService.UpdateAsync(id, updateFundDto);
            return Ok(updated);
        }

        // DELETE /api/funds/{id}
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _fundService.DeleteAsync(id);
            return NoContent();
        }

        // TODO: Maybe it should be a public API
        // GET /api/funds/{id}/nav-history
        [HttpGet("{id}/nav-history")]
        public async Task<IActionResult> GetFundNAVHistoryByFundId(int id)
        {
            var histories = await _fundService.GetFundNAVHistoryByFundIdAsync(id);
            return Ok(histories);
        }

        // TODO: Maybe it should be a public API
        // GET /api/funds/{id}/nav-history/stream
        [HttpGet("{id}/nav-history/stream")]
        public async IAsyncEnumerable<NAVRecordDto> GetStreamFundNAVHistoryByFundId(int id)
        {
            await foreach (var record in _fundService.GetStreamFundNAVHistoryByFundIdAsync(id))
                yield return record;
        }
    }
}
