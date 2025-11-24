namespace FlightSystem.Models
{
    public class Airport
    {
        public int Id { get; set; }
        public string IATA { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
    }
}

