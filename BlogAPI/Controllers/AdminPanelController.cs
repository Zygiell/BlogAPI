using BlogAPI.Models;
using BlogAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlogAPI.Controllers
{
    [Route("api/account/admin")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class AdminPanelController : ControllerBase
    {
        private readonly IAdminPanelService _adminPanelService;

        public AdminPanelController(IAdminPanelService adminPanelService)
        {
            _adminPanelService = adminPanelService;
        }

        [HttpPut("edituser")]
        public async Task<IActionResult> AdminEditUserByIdAsync([FromBody] AdminEditUserDto dto)
        {
            await _adminPanelService.AdminEditUserAsync(dto);
            return Ok();
        }

        [HttpPut("changerole")]
        public async Task<IActionResult> AdminChangeUserRoleAsync([FromQuery] int userId, [FromQuery] int roleToBeId)
        {
            await _adminPanelService.AdminChangeUserRoleAsync(userId, roleToBeId);
            return Ok();
        }

        [HttpDelete("deleteuser")]
        public async Task<IActionResult> AdminRemoveUserAsync([FromQuery] int userId)
        {
            await _adminPanelService.AdminDeleteUserAsync(userId);
            return Ok();
        }

        [HttpGet("getusers")]
        public async Task<IActionResult> AdminGetAllUsersAsync()
        {
            var users = await _adminPanelService.AdminGetAllUsersAsync();
            return Ok(users);
        }

        [HttpGet("getusers/{userId}")]
        public async Task<IActionResult> AdminGetUserByIdAsync([FromRoute] int userId)
        {
            var user = await _adminPanelService.AdminGetUserByIdAsync(userId);
            return Ok(user);
        }
    }
}