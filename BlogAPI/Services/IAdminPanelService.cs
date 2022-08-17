using BlogAPI.Models;

namespace BlogAPI.Services
{
    public interface IAdminPanelService
    {
        void AdminChangeUserRole(int userId, int roleToChangeId);
        void AdminDeleteUser(int userId);
        UserDto AdminGetUserById(int userId);
        IEnumerable<UserDto> AdminGetAllUsers();
        void AdminEditUser(AdminEditUserDto dto);
    }
}