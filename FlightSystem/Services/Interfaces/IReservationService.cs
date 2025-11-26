using FlightSystem.DTOs.Reservation;

public interface IReservationService
{  
    Task<List<ReservationGetDTO>> GetAllAsync();
    Task<ReservationGetDTO?> GetByIdAsync(int id);
    Task<ReservationGetDTO> CreateAsync(ReservationAddDTO dto);
    Task<ReservationGetDTO?> UpdateAsync(ReservationUpdateDTO dto);
    Task<bool> DeleteAsync(int id);
}

