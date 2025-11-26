using FlightSystem.Data;
using FlightSystem.DTOs.User;
using FlightSystem.Models;
using FlightSystem.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace FlightSystem.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _db;

        public UserService(ApplicationDbContext db)
        {
            _db = db;
        }

        // SHA256 hashing
        private string HashPassword(string password, string salt)
        {
            using var sha = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(password + salt);
            return Convert.ToBase64String(sha.ComputeHash(bytes));
        }

        private string GenerateSalt()
        {
            return Guid.NewGuid().ToString("N");
        }

        public async Task<List<UserGetDTO>> GetAllAsync()
        {
            return await _db.Users
                .Select(u => new UserGetDTO
                {
                    Id = u.Id,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Username = u.Username,
                    Email = u.Email,
                    PhoneNumber = u.PhoneNumber,
                    Role = u.Role,
                    IsActive = u.IsActive,
                    IsDeleted = u.IsDeleted
                })
                .ToListAsync();
        }

        public async Task<UserGetDTO?> GetByIdAsync(int id)
        {
            var u = await _db.Users.FindAsync(id);
            if (u == null) return null;

            return new UserGetDTO
            {
                Id = u.Id,
                FirstName = u.FirstName,
                LastName = u.LastName,
                Username = u.Username,
                Email = u.Email,
                PhoneNumber = u.PhoneNumber,
                Role = u.Role,
                IsActive = u.IsActive,
                IsDeleted = u.IsDeleted
            };
        }

        public async Task<UserGetDTO> CreateAsync(UserAddDTO dto)
        {
            string salt = GenerateSalt();
            string hash = HashPassword(dto.Password, salt);

            var user = new FlightSystem.Models.User
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Username = dto.Username,
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber,
                Role = dto.Role,
                PasswordSalt = salt,
                PasswordHash = hash
            };

            _db.Users.Add(user);
            await _db.SaveChangesAsync();

            return new UserGetDTO
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Username = user.Username,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Role = user.Role,
                IsActive = user.IsActive,
                IsDeleted = user.IsDeleted
            };
        }

        public async Task<UserGetDTO?> UpdateAsync(UserUpdateDTO dto)
        {
            var u = await _db.Users.FindAsync(dto.Id);
            if (u == null) return null;

            if (dto.FirstName != null) u.FirstName = dto.FirstName;
            if (dto.LastName != null) u.LastName = dto.LastName;
            if (dto.Email != null) u.Email = dto.Email;
            if (dto.PhoneNumber != null) u.PhoneNumber = dto.PhoneNumber;
            if (dto.Role != null) u.Role = dto.Role;

            if (dto.IsActive.HasValue) u.IsActive = dto.IsActive.Value;
            if (dto.IsDeleted.HasValue) u.IsDeleted = dto.IsDeleted.Value;

            await _db.SaveChangesAsync();

            return new UserGetDTO
            {
                Id = u.Id,
                FirstName = u.FirstName,
                LastName = u.LastName,
                Username = u.Username,
                Email = u.Email,
                PhoneNumber = u.PhoneNumber,
                Role = u.Role,
                IsActive = u.IsActive,
                IsDeleted = u.IsDeleted
            };
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var u = await _db.Users.FindAsync(id);
            if (u == null) return false;

            u.IsDeleted = true;
            await _db.SaveChangesAsync();
            return true;
        }
    }
}

