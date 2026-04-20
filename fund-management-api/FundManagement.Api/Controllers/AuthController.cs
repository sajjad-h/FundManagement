using FundManagement.Api.DTOs.Auth;
using FundManagement.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FundManagement.Api.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            await _authService.RegisterAsync(dto.Email, dto.Password);
            return Ok();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            var token = await _authService.LoginAsync(dto.Email, dto.Password);

            return Ok(new
            {
                token
            });
        }
    }
}
