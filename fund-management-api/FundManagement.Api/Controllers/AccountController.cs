using FundManagement.Api.Data.Interfaces;
using FundManagement.Api.Models;
using FundManagement.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace FundManagement.Api.Controllers
{
    [ApiController]
    [Route("api/accounts")]
    public class AccountController : ControllerBase
    {
        private readonly AccountService _accountService;

        public AccountController(AccountService accountService)
        {
            _accountService = accountService;
        }

        // GET /api/accounts
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var accounts = await _accountService.GetAllAsync();
            return Ok(accounts);
        }

        // GET /api/accounts/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var account = await _accountService.GetByIdAsync(id);
            if (account == null) return NotFound();
            return Ok(account);
        }

        // POST /api/accounts
        [HttpPost]
        public async Task<IActionResult> Create(Account account)
        {
            // TODO: Admin check later
            var created = await _accountService.AddAsync(account);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }
    }
}
