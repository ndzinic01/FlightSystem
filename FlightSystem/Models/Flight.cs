namespace FlightSystem.Models
{
    public class Flight
    {
        public int Id { get; set; }
        public string FlightNumber { get; set; } = string.Empty;

        public int OriginAirportId { get; set; }
        public Airport? OriginAirport { get; set; }

        public int DestinationAirportId { get; set; }
        public Airport? DestinationAirport { get; set; }

        public int AircraftId { get; set; }
        public Aircraft? Aircraft { get; set; }

        public TimeSpan Duration { get; set; }
        public decimal BasePrice { get; set; }

        public ICollection<FlightSchedule>? Schedules { get; set; }
    }
}


