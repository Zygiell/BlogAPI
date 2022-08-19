using BlogAPI.Models;

namespace BlogAPI.Services
{
    public interface IAdminPanelService
    {
        void AdminEditUser(AdminEditUserDto dto);

        void AdminChangeUserRole(int userId, int roleToChangeId);

        void AdminDeleteUser(int userId);

        IEnumerable<UserDto> AdminGetAllUsers();

        UserDto AdminGetUserById(int userId);
    }
}