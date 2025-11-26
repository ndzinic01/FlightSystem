using FlightSystem.DTOs.Destination;

public interface IDestinationService
{
    Task<List<DestinationGetDTO>> GetAllAsync();
    Task<DestinationGetDTO?> GetByIdAsync(int id);
    Task<DestinationGetDTO> CreateAsync(DestinationAddDTO dto);
    Task<DestinationGetDTO?> UpdateAsync(DestinationUpdateDTO dto);
    Task<bool> DeleteAsync(int id);
}

