using FlightSystem.DTOs.Flight;

namespace FlightSystem.Services
{
    public interface IFlightService
    {
        List<FlightGetDto> GetAll();
        FlightGetDto? GetById(int id);
        FlightGetDto Create(FlightCreateDto dto);
        FlightGetDto? Update(int id, FlightCreateDto dto);
        bool Delete(int id);
    }
}
