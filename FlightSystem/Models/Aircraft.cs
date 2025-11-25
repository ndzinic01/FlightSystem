using System.ComponentModel.DataAnnotations;

namespace FlightSystem.Models
{
    public class Aircraft
    {
        [Key]
        public int Id { get; set; }
        public string Model { get; set; } = string.Empty;
        public string RegistrationNumber {  get; set; } = string.Empty;
        public int YearManufacturer { get; set; }
        public string Manufacturer { get; set; } = string.Empty;
        public int Capacity { get; set; }
        public bool Status { get; set; }
        public ICollection<Flight>? Flights { get; set; }
    }
}


