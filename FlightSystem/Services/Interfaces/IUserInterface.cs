using FlightSystem.DTOs.Login;
using FlightSystem.DTOs.User;

namespace FlightSystem.Services.Interfaces
{
    public interface IUserService
    {
        Task<List<UserGetDTO>> GetAllAsync();
        Task<UserGetDTO?> GetByIdAsync(int id);
        Task<UserGetDTO> RegisterAsync(UserAddDTO dto);
        Task<UserGetDTO?> UpdateAsync(UserUpdateDTO dto);
        Task<bool> DeleteAsync(int id);
        Task<LoginResponseDTO?> LoginAsync(LoginDTO dto, params string[] allowedRoles);
    }
}
