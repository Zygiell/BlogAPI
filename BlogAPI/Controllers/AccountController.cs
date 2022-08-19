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
        public ActionResult Login([FromBody] LoginDto dto)
        {
            string token = _accountService.LoginAndGenerateJwt(dto);
            return Ok(token);
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public ActionResult RegisterUser([FromBody] RegisterUserDto dto)
        {
            _accountService.RegisterUser(dto);
            return Ok();
        }

        [HttpPut("editdetails/{userId}")]
        public ActionResult EditUserDetails([FromBody] EditUserDetailsDto dto, [FromRoute] int userId)
        {
            _accountService.EditUserDetails(dto, userId);
            return Ok();
        }

        [HttpDelete("deleteaccount/{userId}")]
        public ActionResult DeleteMyAccount([FromRoute] int userId)
        {
            _accountService.DeleteMyAccount(userId);
            return NoContent();
        }
    }
}