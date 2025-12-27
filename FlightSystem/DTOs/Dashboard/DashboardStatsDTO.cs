namespace FlightSystem.DTOs.Dashboard
{
    public class DashboardStatsDTO
    {
        public int TotalFlights { get; set; }
        public int ActiveUsers { get; set; }
        public int TodayReservations { get; set; }

        public Dictionary<string, int> FlightStatuses { get; set; }
    }
}
