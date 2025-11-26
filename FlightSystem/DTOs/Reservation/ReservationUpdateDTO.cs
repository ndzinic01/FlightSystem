using FlightSystem.Models;

namespace FlightSystem.DTOs.Reservation
{
    public class ReservationUpdateDTO
    {
        public int Id { get; set; }

        public string? SeatNumber { get; set; }
        public int? AdditionalBaggageId { get; set; }
        public StatusReservation? Status { get; set; }
    }

}
