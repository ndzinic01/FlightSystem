namespace FlightSystem.DTOs.Airports
{
    public class AirportGetDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public int CityId { get; set; }
        public string CityName { get; set; } = string.Empty;

        public bool IsActive { get; set; }
        public string Code { get; set; } = string.Empty;
    }
}
