using BlogAPI.Models;

namespace BlogAPI.Services
{
    public interface IAdminPanelService
    {
        Task AdminEditUserAsync(AdminEditUserDto dto);

        Task AdminChangeUserRoleAsync(int userId, int roleToChangeId);

        Task AdminDeleteUserAsync(int userId);

        Task<IEnumerable<UserDto>> AdminGetAllUsersAsync();

        Task<UserDto> AdminGetUserByIdAsync(int userId);
    }
}