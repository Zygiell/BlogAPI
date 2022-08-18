using BlogAPI.Entities;
using BlogAPI.Models;

namespace BlogAPI.Services
{
    public interface IAccountService
    {
        void RegisterUser(RegisterUserDto dto);
        string LoginAndGenerateJwt(LoginDto dto);
        void EditUserDetails(EditUserDetailsDto dto, int userId);
        void DeleteMyAccount(int userId);

    }
}