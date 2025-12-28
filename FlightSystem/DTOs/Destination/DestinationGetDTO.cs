namespace FlightSystem.DTOs.Destination
{
    public class DestinationGetDTO
    {
        public int Id { get; set; }

        public string FromCity { get; set; }
        public string ToCity { get; set; }

        public string FromAirportCode { get; set; }
        public string ToAirportCode { get; set; }

        public bool IsActive { get; set; }
    }

}
