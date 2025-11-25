using FlightSystem.DTOs.Airports;

namespace FlightSystem.Services
{
    public interface IAirportService
    {
        Task<List<AirportGetDTO>> GetAllAsync();
        Task<AirportGetDTO?> GetByIdAsync(int id);
        Task<AirportGetDTO> AddAsync(AirportAddUpdateDTO dto);
        Task<AirportGetDTO?> UpdateAsync(int id, AirportAddUpdateDTO dto);
        Task<bool> DeleteAsync(int id);
    }
}

