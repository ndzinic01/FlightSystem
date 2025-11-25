using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FlightSystem.Models
{
    public class AdditionalBaggage
    {
        [Key]
        public int Id { get; set; }
        public string Type { get; set; } =  string.Empty;

        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }
    }
}
