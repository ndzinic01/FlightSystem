using FlightSystem.DTOs.Country;

namespace FlightSystem.Services
{
    public interface ICountryService
    {
        Task<List<CountryGetDTO>> GetAll();
        Task<CountryGetDTO>GetById(int id);
        Task<CountryGetDTO>Create(CountryAddUpdateDTO dto);
        Task<CountryGetDTO>Update(int id,CountryAddUpdateDTO dto);
        Task<bool>Delete(int id);
    }
}
