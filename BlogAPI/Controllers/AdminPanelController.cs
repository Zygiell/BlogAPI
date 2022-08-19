using BlogAPI.Entities;
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
        public ActionResult AdminEditUserById([FromBody] AdminEditUserDto dto)
        {
            _adminPanelService.AdminEditUser(dto);
            return Ok();
        }

        [HttpPut("changerole")]
        public ActionResult AdminChangeUserRole([FromQuery] int userId, [FromQuery] int roleToBeId)
        {
            _adminPanelService.AdminChangeUserRole(userId, roleToBeId);
            return Ok();
        }

        [HttpDelete("deleteuser")]
        public ActionResult AdminRemoveUser([FromQuery] int userId)
        {
            _adminPanelService.AdminDeleteUser(userId);
            return Ok();
        }

        [HttpGet("getusers")]
        public ActionResult<IEnumerable<User>> AdminGetAllUsers()
        {
            var users = _adminPanelService.AdminGetAllUsers();
            return Ok(users);
        }

        [HttpGet("getusers/{userId}")]
        public ActionResult AdminGetUserById([FromRoute] int userId)
        {
            var user = _adminPanelService.AdminGetUserById(userId);
            return Ok(user);
        }
    }
}