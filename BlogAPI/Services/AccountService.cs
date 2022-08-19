using BlogAPI.Entities;
using BlogAPI.Exceptions;
using BlogAPI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BlogAPI.Services
{
    public class AccountService : IAccountService
    {
        private readonly BlogDbContext _dbContext;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly AuthenticationSettings _authenticationSettings;

        public AccountService(BlogDbContext dbContext, IPasswordHasher<User> passwordHasher, AuthenticationSettings authenticationSettings)
        {
            _dbContext = dbContext;
            _passwordHasher = passwordHasher;
            _authenticationSettings = authenticationSettings;
        }

        public string LoginAndGenerateJwt(LoginDto dto)
        {
            var user = _dbContext.Users
                .Include(u => u.Role)
                .FirstOrDefault(u => u.Email == dto.Email);

            if (user is null)
            {
                throw new BadRequestException("Invalid username or password");
            }

            var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, dto.Password);
            if (result == PasswordVerificationResult.Failed)
            {
                throw new BadRequestException("Invalid username or password");
            }

            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, $"{user.FirstName} {user.LastName}"),
                new Claim(ClaimTypes.Role, $"{user.Role.Name}"),
                new Claim("City", user.City)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authenticationSettings.JwtKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(_authenticationSettings.JwtExpireDays);

            var token = new JwtSecurityToken(_authenticationSettings.JwtIssuer,
                _authenticationSettings.JwtIssuer,
                claims,
                expires: expires,
                signingCredentials: credentials);

            var tokenHandler = new JwtSecurityTokenHandler();

            return tokenHandler.WriteToken(token);
        }

        public void RegisterUser(RegisterUserDto dto)
        {
            var userRole = _dbContext.Roles.FirstOrDefault(r => r.Name == "User");
            var roleId = userRole.Id;

            var newUser = new User()
            {
                Email = dto.Email,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                City = dto.City,
                RoleId = roleId
            };
            var hashedPassword = _passwordHasher.HashPassword(newUser, dto.Password);
            newUser.PasswordHash = hashedPassword;

            _dbContext.Users.Add(newUser);
            _dbContext.SaveChanges();
        }

        public void EditUserDetails(EditUserDetailsDto dto, int userId)
        {
            var userToBeEdited = FindUserById(userId);

            if (dto.FirstName.Length > 0)
            {
                userToBeEdited.FirstName = dto.FirstName;
            }
            if (dto.LastName.Length > 0)
            {
                userToBeEdited.LastName = dto.LastName;
            }
            if (dto.City.Length > 0)
            {
                userToBeEdited.City = dto.City;
            }
            if (dto.Password.Length > 0)
            {
                var hashedPassword = _passwordHasher.HashPassword(userToBeEdited, dto.Password);
                userToBeEdited.PasswordHash = hashedPassword;
            }

            _dbContext.SaveChanges();
        }

        public void DeleteMyAccount(int userId)
        {
            var userToBeDeleted = FindUserById(userId);

            _dbContext.Remove(userToBeDeleted);
            _dbContext.SaveChanges();
        }

        /// <summary>
        /// Finds specific user in database and returns it as a value
        /// </summary>
        /// <param name="userId">User id</param>
        /// <returns>User as a value</returns>
        /// <exception cref="NotFoundException">Throws when user id dosent exist in database</exception>
        private User FindUserById(int userId)
        {
            var user = _dbContext
                .Users
                .Include(u => u.Role)
                .FirstOrDefault(r => r.Id == userId);

            if (user == null)
            {
                throw new NotFoundException($"User with {userId} id does not exist");
            }

            return user;
        }
    }
}