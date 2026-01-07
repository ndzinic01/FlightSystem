namespace FlightSystem.DTOs.Airports
{
    public class AirportAddUpdateDTO
    {
        public string Name { get; set; } = string.Empty;
        public int CityId { get; set; }
        public bool IsActive { get; set; }
        public string Code { get; set; } = string.Empty;
    }
}
