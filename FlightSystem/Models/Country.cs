using System.ComponentModel.DataAnnotations;

namespace FlightSystem.Models
{
    public class Country
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public ICollection<City>?Cities { get; set; }
    }
}
