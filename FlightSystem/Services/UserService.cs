using FlightSystem.Data;
using FlightSystem.DTOs.User;
using FlightSystem.DTOs.Login;
using FlightSystem.Models;
using FlightSystem.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
namespace FlightSystem.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _db;
        private readonly IConfiguration _config;
        public UserService(ApplicationDbContext db, IConfiguration config)
        {
            _db = db;
            _config = config;
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

        public async Task<UserGetDTO?> RegisterAsync(UserAddDTO dto)
        {
            // Provjeri da li već postoji username ili email
            if (await _db.Users.AnyAsync(u => u.Username == dto.Username))
                throw new Exception("Username already exists.");

            if (await _db.Users.AnyAsync(u => u.Email == dto.Email))
                throw new Exception("Email already exists.");

            string salt = GenerateSalt();
            string hash = HashPassword(dto.Password, salt);

            var user = new User
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Username = dto.Username,
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber,
                Role = "Customer", // default role
                PasswordSalt = salt,
                PasswordHash = hash,
                IsActive = true,
                IsDeleted = false
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

        public async Task<LoginResponseDTO?> LoginAsync(LoginDTO dto, params string[] allowedRoles)
        {
            var user = await _db.Users.FirstOrDefaultAsync(x => x.Username == dto.Username);

            if (user == null || user.IsDeleted || !user.IsActive)
                return null;

            string hash = HashPassword(dto.Password, user.PasswordSalt);
            if (hash != user.PasswordHash)
                return null;

            // Provjera rola
            if (allowedRoles != null && allowedRoles.Length > 0 && !allowedRoles.Contains(user.Role))
                return null;

            // === JWT Token ===
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_config["Jwt:Key"])
            );

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddDays(int.Parse(_config["Jwt:ExpireDays"])),
                signingCredentials: creds
            );

            string jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return new LoginResponseDTO
            {
                Token = jwt,
                User = new UserGetDTO
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
                }
            };
        }

    }
}

