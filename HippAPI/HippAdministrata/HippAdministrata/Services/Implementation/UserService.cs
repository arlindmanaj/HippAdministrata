﻿using HippAdministrata.Data;
using HippAdministrata.Models.Domains;
using HippAdministrata.Models.DTOs;
using HippAdministrata.Repositories.Interface;
using HippAdministrata.Services.Interface;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace HippAdministrata.Services.Implementation
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly ApplicationDbContext _dbContext;

        public UserService(IUserRepository userRepository, ApplicationDbContext dbContext)
        {
            _userRepository = userRepository;
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<UserDTO>> GetAllUsersAsync()
        {
            var users = await _userRepository.GetAllUsersAsync();
            return users.Select(user => new UserDTO
            {
                UserId = user.UserId,
                Name = user.Name,
                Role = user.Role?.RoleName ?? "Unknown" // Handle potential null Role
            });
        }

        public async Task<UserDTO> GetUserByIdAsync(int id)
        {
            var user = await _userRepository.GetUserByIdAsync(id);
            if (user == null) return null;

            return new UserDTO
            {
                UserId = user.UserId,
                Name = user.Name,
                Role = user.Role?.RoleName ?? "Unknown"
            };
        }

        public async Task<bool> AddUserAsync(User model)
        {
            var user = new User
            {
                Name = model.Name,
                PasswordHash = model.PasswordHash, // Remember to hash passwords
                Role = model.Role,
                Email = model.Email
            };

            await _userRepository.AddUserAsync(user);
            return true;
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            var user = await _userRepository.GetUserByIdAsync(id);
            if (user == null) return false;

            await _userRepository.DeleteUserAsync(user);
            return true;
        }
        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(hashedBytes);
        }
        public async Task<bool> ChangeUserRoleAsync(int id, string newRole, string newName)
        {
            var user = await _userRepository.GetUserByIdAsync(id);
            if (user == null) return false;

            var role = await _dbContext.Roles.FirstOrDefaultAsync(r => r.RoleName == newRole);
            if (role == null) return false;

            user.Role = role; // Assign the Role object
            user.Name = newName;

            await _userRepository.UpdateUserAsync(user);
            return true;
        }
    }
}
