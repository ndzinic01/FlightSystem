using FlightSystem.Data;
using FlightSystem.DTOs.Notification;
using FlightSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace FlightSystem.Services
{
    public class NotificationService : INotificationService
    {
        private readonly ApplicationDbContext _db;

        public NotificationService(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<List<NotificationGetDTO>> GetAllAsync()
        {
            return await _db.Notifications
                .Include(n => n.User)
                .Include(n => n.Flight)
                .Select(n => new NotificationGetDTO
                {
                    Id = n.Id,
                    UserId = n.UserId,
                    UserFullName = n.User.FirstName + " " + n.User.LastName,
                    FlightId = n.FlightId,
                    FlightCode = n.Flight.Code,
                    Message = n.Message,
                    SentAt = n.SentAt,
                    IsRead = n.IsRead
                })
                .ToListAsync();
        }

        public async Task<List<NotificationGetDTO>> GetByUserAsync(int userId)
        {
            return await _db.Notifications
                .Where(n => n.UserId == userId)
                .Include(n => n.User)
                .Include(n => n.Flight)
                .Select(n => new NotificationGetDTO
                {
                    Id = n.Id,
                    UserId = n.UserId,
                    UserFullName = n.User.FirstName + " " + n.User.LastName,
                    FlightId = n.FlightId,
                    FlightCode = n.Flight.Code,
                    Message = n.Message,
                    SentAt = n.SentAt,
                    IsRead = n.IsRead
                })
                .ToListAsync();
        }

        public async Task<NotificationGetDTO?> GetByIdAsync(int id)
        {
            var n = await _db.Notifications
                .Include(x => x.User)
                .Include(x => x.Flight)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (n == null) return null;

            return new NotificationGetDTO
            {
                Id = n.Id,
                UserId = n.UserId,
                UserFullName = n.User.FirstName + " " + n.User.LastName,
                FlightId = n.FlightId,
                FlightCode = n.Flight.Code,
                Message = n.Message,
                SentAt = n.SentAt,
                IsRead = n.IsRead
            };
        }

        public async Task<NotificationGetDTO> CreateAsync(NotificationAddDTO dto)
        {
            var n = new FlightSystem.Models.Notification
            {
                UserId = dto.UserId,
                FlightId = dto.FlightId,
                Message = dto.Message,
                SentAt = DateTime.UtcNow,
                IsRead = false
            };

            _db.Notifications.Add(n);
            await _db.SaveChangesAsync();

            var user = await _db.Users.FindAsync(dto.UserId);
            var flight = await _db.Flights.FindAsync(dto.FlightId);

            return new NotificationGetDTO
            {
                Id = n.Id,
                UserId = dto.UserId,
                UserFullName = $"{user?.FirstName} {user?.LastName}",
                FlightId = dto.FlightId,
                FlightCode = flight?.Code ?? "",
                Message = dto.Message,
                SentAt = n.SentAt,
                IsRead = false
            };
        }

        public async Task<NotificationGetDTO?> UpdateAsync(NotificationUpdateDTO dto)
        {
            var n = await _db.Notifications.FindAsync(dto.Id);
            if (n == null) return null;

            n.IsRead = dto.IsRead;

            await _db.SaveChangesAsync();

            var user = await _db.Users.FindAsync(n.UserId);
            var flight = await _db.Flights.FindAsync(n.FlightId);

            return new NotificationGetDTO
            {
                Id = n.Id,
                UserId = n.UserId,
                UserFullName = $"{user?.FirstName} {user?.LastName}",
                FlightId = n.FlightId,
                FlightCode = flight?.Code ?? "",
                Message = n.Message,
                SentAt = n.SentAt,
                IsRead = n.IsRead
            };
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var n = await _db.Notifications.FindAsync(id);
            if (n == null) return false;

            _db.Notifications.Remove(n);
            await _db.SaveChangesAsync();

            return true;
        }
    }
}


