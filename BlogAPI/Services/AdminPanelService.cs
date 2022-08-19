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

        public void AdminEditUser(AdminEditUserDto dto)
        {
            var userToBeEdited = FindUserById(dto.Id);

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

            _dbContext.SaveChanges();
        }

        public void AdminChangeUserRole(int userId, int roleToChangeId)
        {
            var userToChange = FindUserById(userId);
            userToChange.RoleId = roleToChangeId;
            _dbContext.SaveChanges();
        }

        public void AdminDeleteUser(int userId)
        {
            var userToBeDeleted = FindUserById(userId);
            _dbContext.Remove(userToBeDeleted);
            _dbContext.SaveChanges();
        }

        public IEnumerable<UserDto> AdminGetAllUsers()
        {
            var users = _dbContext
                .Users
                .Include(u => u.Role)
                .ToList();

            if (!users.Any())
            {
                throw new NotFoundException("There are no users");
            }

            var usersDtos = _mapper.Map<IEnumerable<UserDto>>(users);

            return usersDtos;
        }

        public UserDto AdminGetUserById(int userId)
        {
            var user = FindUserById(userId);
            var userDto = _mapper.Map<UserDto>(user);
            return userDto;
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