namespace FlightSystem.DTOs.Destination
{
    public class DestinationUpdateDTO
    {
        public int Id { get; set; }
        public int FromAirportId { get; set; }
        public int ToAirportId { get; set; }
        public bool IsActive { get; set; }
    }

}
