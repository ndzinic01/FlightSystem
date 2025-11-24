namespace FlightSystem.Models
{
    public class FlightSchedule
    {
        public int Id { get; set; }

        public int FlightId { get; set; }
        public Flight? Flight { get; set; }

        public DateTime Date { get; set; }
        public int AvailableSeats { get; set; }

        public ICollection<Reservation>? Reservations { get; set; }
    }
}


