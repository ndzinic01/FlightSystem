using System.ComponentModel.DataAnnotations.Schema;

namespace FlightSystem.Models
{
    public enum FlightStatus
    {
        Scheduled,
        Boarding,
        Delayed,
        Cancelled,
        Completed
    }
    public class Flight
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public int DestinationId { get; set; }
        [ForeignKey(nameof(DestinationId))]
        public Destination Destination { get; set; }
        public int AirlineId { get; set; }
        [ForeignKey(nameof(AirlineId))]
        public Airline Airline { get; set; }
        public int AircraftId { get; set; }
        [ForeignKey(nameof(AircraftId))]
        public Aircraft Aircraft { get; set; }
        public DateTime DepartureTime { get; set; }
        public DateTime ArrivalTime { get; set; }
        public FlightStatus Status { get; set; } = FlightStatus.Scheduled;

        [Column(TypeName ="decimal(18,2)")]
        public decimal Price { get; set; }
        public int AvailableSeats { get; set; }

        
    }
}


