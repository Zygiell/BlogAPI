using BlogAPI.Models;

namespace BlogAPI.Services
{
    public interface IAccountService
    {
        Task RegisterUserAsync(RegisterUserDto dto);

        Task<string> LoginAndGenerateJwtAsync(LoginDto dto);

        Task EditUserDetailsAsync(EditUserDetailsDto dto, int userId);

        Task DeleteMyAccountAsync(int userId);
    }
}