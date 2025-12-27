using FlightSystem.DTOs.Dashboard;

namespace FlightSystem.Services
{
    public interface IDashboardService
    {
        Task<DashboardStatsDTO> GetStatsAsync();
    }
}

