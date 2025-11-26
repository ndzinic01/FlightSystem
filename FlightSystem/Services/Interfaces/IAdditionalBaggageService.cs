using FlightSystem.DTOs.AdditionalBaggage;

public interface IAdditionalBaggageService
{
    Task<List<AdditionalBaggageGetDTO>> GetAllAsync();
    Task<AdditionalBaggageGetDTO?> GetByIdAsync(int id);
    Task<AdditionalBaggageGetDTO> CreateAsync(AdditionalBaggageAddDTO dto);
    Task<AdditionalBaggageGetDTO?> UpdateAsync(AdditionalBaggageUpdateDTO dto);
    Task<bool> DeleteAsync(int id);
}

