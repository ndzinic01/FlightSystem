namespace FlightSystem.DTOs.Airports
{
    public class AirportCreateDto
    {
        public string IATA { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
    }
}

