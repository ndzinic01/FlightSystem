using System.Net.Sockets;

namespace FlightSystem.Models
{
    public class Reservation
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        public User? User { get; set; }

        public int FlightScheduleId { get; set; }
        public FlightSchedule? FlightSchedule { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<Ticket>? Tickets { get; set; }
    }
}

