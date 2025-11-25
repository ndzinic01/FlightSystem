using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FlightSystem.Models
{
    public class Destination
    {
        [Key]
        public int Id { get; set; }
        public int FromAirportId { get; set; }
        [ForeignKey(nameof(FromAirportId))]
        public Airport FromAirport { get; set; }
        public int ToAirportId { get; set; }
        [ForeignKey(nameof(ToAirportId))]
        public Airport ToAirport { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
