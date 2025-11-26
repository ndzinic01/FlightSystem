using FlightSystem.Models;

namespace FlightSystem.DTOs.Reservation
{
    public class ReservationAddDTO
    {
        public int UserId { get; set; }
        public int FlightId { get; set; }
        public string SeatNumber { get; set; }

        public int? AdditionalBaggageId { get; set; }

        public StatusReservation Status { get; set; } = StatusReservation.Confirmed;
    }

}
