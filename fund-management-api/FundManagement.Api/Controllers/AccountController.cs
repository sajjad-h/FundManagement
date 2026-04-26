using FundManagement.Api.DTOs.Account;
using FundManagement.Api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FundManagement.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/accounts")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        // GET /api/accounts
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var accounts = await _accountService.GetAllAsync();
            return Ok(accounts);
        }

        // TODO: user can see only his account not others
        // GET /api/accounts/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var account = await _accountService.GetByIdAsync(id);
            if (account == null) return NotFound();
            return Ok(account);
        }

        /**
         * TODO: Admin can create account for other user,
         *       Investor can create account only for himself/herself
         */
        // POST /api/accounts
        [HttpPost]
        public async Task<IActionResult> Create(CreateAccountDto createAccountDto)
        {
            // TODO: Admin check later
            var created = await _accountService.AddAsync(createAccountDto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }
    }
}
