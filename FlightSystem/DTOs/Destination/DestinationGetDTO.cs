namespace FlightSystem.DTOs.Destination
{
    public class DestinationGetDTO
    {
        public int Id { get; set; }
        public int FromAirportId { get; set; }
        public string FromAirportName { get; set; }
        public int ToAirportId { get; set; }
        public string ToAirportName { get; set; }
        public bool IsActive { get; set; }
    }

}
