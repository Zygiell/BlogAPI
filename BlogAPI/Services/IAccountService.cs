using BlogAPI.Models;

namespace BlogAPI.Services
{
    public interface IAccountService
    {
        void RegisterUser(RegisterUserDto dto);
        string GenerateJwt(LoginDto dto);
        void ChangeUserRole(int userId, int roleToChangeId);
    }
}