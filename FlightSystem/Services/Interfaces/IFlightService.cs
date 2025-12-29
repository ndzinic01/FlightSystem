using FlightSystem.DTOs.Flight;

namespace FlightSystem.Services
{
    public interface IFlightService
    {
        Task<List<FlightGetDTO>> GetAll();
        Task<FlightGetDTO?> GetById(int id);
        Task<FlightGetDTO> Create(FlightCreateDTO dto);
        Task<FlightGetDTO?> Update(int id, FlightUpdateDTO dto);
        Task<List<FlightGetDTO>> GetByDestination(int destinationId);

        Task<bool> Delete(int id);
    }
}
