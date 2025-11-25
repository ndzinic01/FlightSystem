namespace FlightSystem.DTOs.Airline
{
    public class AirlineGetDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? LogoURL { get; set; }
    }
}
