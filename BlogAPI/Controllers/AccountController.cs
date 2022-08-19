using BlogAPI.Models;
using BlogAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlogAPI.Controllers
{// IN DEVELOPMENT
    [Route("api/account")]
    [ApiController]
    [Authorize]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> LoginAsync([FromBody] LoginDto dto)
        {
            string token = await _accountService.LoginAndGenerateJwtAsync(dto);
            return Ok(token);
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> RegisterUserAsync([FromBody] RegisterUserDto dto)
        {
            await _accountService.RegisterUserAsync(dto);
            return Ok();
        }

        [HttpPut("editdetails/{userId}")]
        public async Task<IActionResult> EditUserDetailsAsync([FromBody] EditUserDetailsDto dto, [FromRoute] int userId)
        {
            await _accountService.EditUserDetailsAsync(dto, userId);
            return Ok();
        }

        [HttpDelete("deleteaccount/{userId}")]
        public async Task<IActionResult> DeleteMyAccountAsync([FromRoute] int userId)
        {
            await _accountService.DeleteMyAccountAsync(userId);
            return NoContent();
        }
    }
}