using FlightSystem.DTOs.Airline;

namespace FlightSystem.Services.Interfaces
{
    public interface IAirlineService
    {
        Task<List<AirlineGetDTO>> GetAllAsync();
        Task<AirlineGetDTO?> GetByIdAsync(int id);
        Task<AirlineGetDTO> AddAsync(AirlineAddUpdateDTO dto);
        Task<AirlineGetDTO?> UpdateAsync(int id, AirlineAddUpdateDTO dto);
        Task<bool> DeleteAsync(int id);
    }
}
