using FlightSystem.DTOs.Aircraft;

namespace FlightSystem.Services
{
    public interface IAircraftService
    {
        Task<List<AircraftGetDTO>> GetAllAsync();
        Task<AircraftGetDTO?> GetByIdAsync(int id);
        Task<AircraftGetDTO> AddAsync(AircraftAddUpdateDTO dto);
        Task<AircraftGetDTO?> UpdateAsync(int id, AircraftAddUpdateDTO dto);
        Task<bool> DeleteAsync(int id);
    }
}
