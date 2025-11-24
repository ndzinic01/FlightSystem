using FlightSystem.DTOs.Aircraft;

namespace FlightSystem.Services.Interfaces
{
    public interface IAircraftService
    {
        List<AircraftGetDto> GetAll();
        AircraftGetDto? GetById(int id);
        AircraftGetDto Create(AircraftCreateDto dto);
        AircraftGetDto? Update(int id, AircraftCreateDto dto);
        bool Delete(int id);
    }
}
