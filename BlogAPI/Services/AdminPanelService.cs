using AutoMapper;
using BlogAPI.Entities;
using BlogAPI.Exceptions;
using BlogAPI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BlogAPI.Services
{
    public class AdminPanelService : IAdminPanelService
    {
        private readonly BlogDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IPasswordHasher<User> _passwordHasher;

        public AdminPanelService(BlogDbContext dbContext, IMapper mapper, IPasswordHasher<User> passwordHasher)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _passwordHasher = passwordHasher;
        }

        public async Task AdminEditUserAsync(AdminEditUserDto dto)
        {
            var userToBeEdited = await FindUserByIdAsync(dto.Id);

            // Prevent changes from empty fields.
            if (dto.Email.Length > 0)
            {
                userToBeEdited.Email = dto.Email;
            }
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

            await _dbContext.SaveChangesAsync();
        }

        public async Task AdminChangeUserRoleAsync(int userId, int roleToChangeId)
        {
            var userToChange = await FindUserByIdAsync(userId);
            userToChange.RoleId = roleToChangeId;
            await _dbContext.SaveChangesAsync();
        }

        public async Task AdminDeleteUserAsync(int userId)
        {
            var userToBeDeleted = await FindUserByIdAsync(userId);
            _dbContext.Remove(userToBeDeleted);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<UserDto>> AdminGetAllUsersAsync()
        {
            var users = await _dbContext
                .Users
                .Include(u => u.Role)
                .ToListAsync();

            if (!users.Any())
            {
                throw new NotFoundException("There are no users");
            }

            var usersDtos = _mapper.Map<IEnumerable<UserDto>>(users);

            return usersDtos;
        }

        public async Task<UserDto> AdminGetUserByIdAsync(int userId)
        {
            var user = await FindUserByIdAsync(userId);
            var userDto = _mapper.Map<UserDto>(user);
            return userDto;
        }

        /// <summary>
        /// Finds specific user in database and returns it as a value
        /// </summary>
        /// <param name="userId">User id</param>
        /// <returns>User as a value</returns>
        /// <exception cref="NotFoundException">Throws when user id dosent exist in database</exception>
        private async Task<User> FindUserByIdAsync(int userId)
        {
            var user = await _dbContext
                .Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(r => r.Id == userId);

            if (user == null)
            {
                throw new NotFoundException($"User with {userId} id does not exist");
            }

            return user;
        }
    }
}