using FlightSystem.DTOs.City;

namespace FlightSystem.Services.Interfaces
{
    public interface ICityService
    {
        Task<List<CityGetDTO>> GetAll();
        Task<CityGetDTO?> GetById(int id);
        Task<CityGetDTO> Create(CityAddUpdateDTO dto);
        Task<CityGetDTO?> Update(int id, CityAddUpdateDTO dto);
        Task<bool> Delete(int id);
    }
}
