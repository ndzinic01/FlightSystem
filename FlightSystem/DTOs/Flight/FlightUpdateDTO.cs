using FlightSystem.Models;

namespace FlightSystem.DTOs.Flight
{
    public class FlightUpdateDTO
    {
        public string Code { get; set; }
        public int DestinationId { get; set; }
        public int AirlineId { get; set; }
        public int AircraftId { get; set; }
        public DateTime DepartureTime { get; set; }
        public DateTime ArrivalTime { get; set; }
        public decimal Price { get; set; }
        public int AvailableSeats { get; set; }
        public FlightStatus Status { get; set; }
    }

}
