using FlightSystem.Data;
using FlightSystem.DTOs.Dashboard;
using Microsoft.EntityFrameworkCore;

namespace FlightSystem.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly ApplicationDbContext _db;

        public DashboardService(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<DashboardStatsDTO> GetStatsAsync()
        {
            var today = DateTime.UtcNow.Date;

            return new DashboardStatsDTO
            {
                TotalFlights = await _db.Flights.CountAsync(),

                ActiveUsers = await _db.Users
                    .CountAsync(u => u.IsActive && !u.IsDeleted),

                TodayReservations = await _db.Reservations
                    .CountAsync(r => r.CreatedAt.Date == today),

                FlightStatuses = await _db.Flights
                    .GroupBy(f => f.Status)
                    .Select(g => new
                    {
                        Status = g.Key.ToString(),
                        Count = g.Count()
                    })
                    .ToDictionaryAsync(x => x.Status, x => x.Count)
            };
        }
    
}
}
