namespace FlightSystem.DTOs.Flight
{
    public class FlightCreateDto
    {
        public string FlightNumber { get; set; } = string.Empty;

        public int OriginAirportId { get; set; }
        public int DestinationAirportId { get; set; }
        public int AircraftId { get; set; }

        public double DurationHours { get; set; }
        public decimal BasePrice { get; set; }
    }
}

