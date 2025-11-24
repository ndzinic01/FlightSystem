namespace FlightSystem.DTOs.Flight
{
    public class FlightGetDto
    {
        public int Id { get; set; }
        public string FlightNumber { get; set; } = string.Empty;

        public int OriginAirportId { get; set; }
        public string OriginAirportName { get; set; } = string.Empty;

        public int DestinationAirportId { get; set; }
        public string DestinationAirportName { get; set; } = string.Empty;

        public int AircraftId { get; set; }
        public string AircraftModel { get; set; } = string.Empty;

        public double DurationHours { get; set; }
        public decimal BasePrice { get; set; }
    }
}

