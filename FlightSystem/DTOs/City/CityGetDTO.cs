namespace FlightSystem.DTOs.City
{
    public class CityGetDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string CountryName {  get; set; } = string.Empty;
        public int CountryId { get; set; }
    }
}
