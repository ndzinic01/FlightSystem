using FlightSystem.Models;

namespace FlightSystem.DTOs.Flight
{
    public class FlightGetDTO
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Destination { get; set; }
        public string Airline { get; set; }
        public string Aircraft { get; set; }
        public DateTime DepartureTime { get; set; }
        public DateTime ArrivalTime { get; set; }
        public FlightStatus Status { get; set; }
        public decimal Price { get; set; }
        public int AvailableSeats { get; set; }
    }

}
