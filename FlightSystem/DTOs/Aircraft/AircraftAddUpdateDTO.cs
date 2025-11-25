namespace FlightSystem.DTOs.Aircraft
{
    public class AircraftAddUpdateDTO
    {
        public string Model { get; set; } = string.Empty;
        public string RegistrationNumber { get; set; } = string.Empty;
        public int YearManufacturer { get; set; }
        public string Manufacturer { get; set; } = string.Empty;
        public int Capacity { get; set; }
        public bool Status { get; set; }
    }
}
