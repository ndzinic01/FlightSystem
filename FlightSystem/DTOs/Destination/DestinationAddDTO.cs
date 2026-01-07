namespace FlightSystem.DTOs.Destination
{
    public class DestinationAddDTO
    {
        public int FromAirportId { get; set; }
        public int ToAirportId { get; set; }
        public bool IsActive { get; set; } = true;
    }

}
