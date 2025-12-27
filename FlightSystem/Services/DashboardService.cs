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
            var now = DateTime.UtcNow;
            var today = now.Date;

            // Početak i kraj trenutnog mjeseca
            var startOfMonth = new DateTime(now.Year, now.Month, 1);
            var endOfMonth = startOfMonth.AddMonths(1).AddDays(-1);

            // Broj rezervacija po danima u trenutnom mjesecu
            var monthlyReservations = await _db.Reservations
                .Where(r => r.CreatedAt.Date >= startOfMonth && r.CreatedAt.Date <= endOfMonth)
                .GroupBy(r => r.CreatedAt.Day)
                .Select(g => new { Day = g.Key, Count = g.Count() })
                .ToDictionaryAsync(x => x.Day, x => x.Count);

            return new DashboardStatsDTO
            {
                TotalFlights = await _db.Flights.CountAsync(),

                ActiveUsers = await _db.Users.CountAsync(u => u.IsActive && !u.IsDeleted),

                TodayReservations = await _db.Reservations.CountAsync(r => r.CreatedAt.Date == today),

                FlightStatuses = await _db.Flights
                    .GroupBy(f => f.Status)
                    .Select(g => new { Status = g.Key.ToString(), Count = g.Count() })
                    .ToDictionaryAsync(x => x.Status, x => x.Count),

                MonthlyReservations = monthlyReservations
            };
        }


    }
}
