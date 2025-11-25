using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net.Sockets;

namespace FlightSystem.Models
{
    public enum StatusReservation
    {
        Confirmed,
        Cancelled,
        Pending
    }
    public class Reservation
    {
        [Key]
        public int Id { get; set; }

        public int UserId { get; set; }
        [ForeignKey(nameof(UserId))]    
        public User User { get; set; }

        public int FlightId { get; set; }
        [ForeignKey(nameof(FlightId))]
        public Flight Flight { get; set; }
        public string SeatNumber { get; set; }
        public int? AdditionalBaggageId { get; set; }
        [ForeignKey(nameof(AdditionalBaggageId))]
        public AdditionalBaggage? AdditionalBaggage { get; set; }
        [Column(TypeName ="decimal(18,2)")]
        public decimal TotalPrice { get; set; }
        public StatusReservation Status { get; set; } = StatusReservation.Confirmed;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    }
}

