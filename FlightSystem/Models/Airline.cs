using System.ComponentModel.DataAnnotations;

namespace FlightSystem.Models
{
    public class Airline
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string? LogoURL { get; set; }
        public ICollection<Flight>? Flights { get; set; }
    }
}
