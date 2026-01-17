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
                .OrderByDescending(n => n.SentAt)
                .Select(n => new NotificationGetDTO
                {
                    Id = n.Id,
                    UserId = n.UserId,
                    UserFullName = n.User != null ? n.User.FirstName + " " + n.User.LastName : "Svi korisnici",
                    UserEmail = n.User != null ? n.User.Email : null,
                    FlightId = n.FlightId,
                    FlightCode = n.Flight != null ? n.Flight.Code : null,
                    Type = n.Type,
                    Message = n.Message,
                    SentAt = n.SentAt,
                    IsRead = n.IsRead,
                    AdminReply = n.AdminReply,
                    IsSystemGenerated = n.IsSystemGenerated
                })
                .ToListAsync();
        }

        public async Task<List<NotificationGetDTO>> GetByUserAsync(int userId)
        {
            return await _db.Notifications
                .Where(n => n.UserId == userId || n.UserId == null) // 🔥 Uključi broadcast poruke
                .Include(n => n.User)
                .Include(n => n.Flight)
                .OrderByDescending(n => n.SentAt)
                .Select(n => new NotificationGetDTO
                {
                    Id = n.Id,
                    UserId = n.UserId,
                    UserFullName = n.User != null ? n.User.FirstName + " " + n.User.LastName : "Sistem",
                    UserEmail = n.User != null ? n.User.Email : null,
                    FlightId = n.FlightId,
                    FlightCode = n.Flight != null ? n.Flight.Code : null,
                    Type = n.Type,
                    Message = n.Message,
                    SentAt = n.SentAt,
                    IsRead = n.IsRead,
                    AdminReply = n.AdminReply,
                    IsSystemGenerated = n.IsSystemGenerated
                })
                .ToListAsync();
        }

        // 🔥 NOVA METODA - Broadcast notifikacija svim korisnicima
        public async Task<int> BroadcastToAllUsersAsync(BroadcastNotificationDTO dto)
        {
            var users = await _db.Users
                .Where(u => u.IsActive && !u.IsDeleted)
                .ToListAsync();

            var notifications = users.Select(user => new Notification
            {
                UserId = user.Id,
                FlightId = dto.FlightId,
                Type = NotificationType.SystemBroadcast,
                Message = dto.Message,
                SentAt = DateTime.UtcNow,
                IsRead = false,
                IsSystemGenerated = true
            }).ToList();

            _db.Notifications.AddRange(notifications);
            await _db.SaveChangesAsync();

            return notifications.Count;
        }

        // 🔥 NOVA METODA - Automatska notifikacija za otkazan let
        public async Task<int> NotifyFlightCancellationAsync(int flightId, string reason)
        {
            var reservations = await _db.Reservations
                .Where(r => r.FlightId == flightId)
                .Include(r => r.User)
                .Include(r => r.Flight)
                    .ThenInclude(f => f.Destination)
                    .ThenInclude(d => d.FromAirport)
                    .ThenInclude(a => a.City)
                .Include(r => r.Flight)
                    .ThenInclude(f => f.Destination)
                    .ThenInclude(d => d.ToAirport)
                    .ThenInclude(a => a.City)
                .ToListAsync();

            if (!reservations.Any()) return 0;

            var flight = reservations.First().Flight;
            var destination = $"{flight.Destination.FromAirport.City.Name} → {flight.Destination.ToAirport.City.Name}";

            var notifications = reservations.Select(r => new Notification
            {
                UserId = r.UserId,
                FlightId = flightId,
                Type = NotificationType.FlightCancellation,
                Message = $"Obavještavamo vas da je let {flight.Code} ({destination}) otkazan. Razlog: {reason}. Molimo kontaktirajte nas za povrat novca ili preusmjeravanje na drugi let.",
                SentAt = DateTime.UtcNow,
                IsRead = false,
                IsSystemGenerated = true
            }).ToList();

            _db.Notifications.AddRange(notifications);
            await _db.SaveChangesAsync();

            return notifications.Count;
        }

        // 🔥 NOVA METODA - Automatska notifikacija za promjenu vremena
        public async Task<int> NotifyFlightRescheduleAsync(int flightId, DateTime newDepartureTime)
        {
            var reservations = await _db.Reservations
                .Where(r => r.FlightId == flightId)
                .Include(r => r.User)
                .Include(r => r.Flight)
                    .ThenInclude(f => f.Destination)
                    .ThenInclude(d => d.FromAirport)
                    .ThenInclude(a => a.City)
                .Include(r => r.Flight)
                    .ThenInclude(f => f.Destination)
                    .ThenInclude(d => d.ToAirport)
                    .ThenInclude(a => a.City)
                .ToListAsync();

            if (!reservations.Any()) return 0;

            var flight = reservations.First().Flight;
            var destination = $"{flight.Destination.FromAirport.City.Name} → {flight.Destination.ToAirport.City.Name}";

            var notifications = reservations.Select(r => new Notification
            {
                UserId = r.UserId,
                FlightId = flightId,
                Type = NotificationType.FlightReschedule,
                Message = $"Obavještavamo vas da je vrijeme polaska leta {flight.Code} ({destination}) promijenjeno. Novo vrijeme: {newDepartureTime:dd.MM.yyyy HH:mm}. Molimo prilagodite svoje planove.",
                SentAt = DateTime.UtcNow,
                IsRead = false,
                IsSystemGenerated = true
            }).ToList();

            _db.Notifications.AddRange(notifications);
            await _db.SaveChangesAsync();

            return notifications.Count;
        }

        // 🔥 NOVA METODA - Automatska notifikacija za zakašnjenje
        public async Task<int> NotifyFlightDelayAsync(int flightId, int delayMinutes)
        {
            var reservations = await _db.Reservations
                .Where(r => r.FlightId == flightId)
                .Include(r => r.User)
                .Include(r => r.Flight)
                    .ThenInclude(f => f.Destination)
                    .ThenInclude(d => d.FromAirport)
                    .ThenInclude(a => a.City)
                .Include(r => r.Flight)
                    .ThenInclude(f => f.Destination)
                    .ThenInclude(d => d.ToAirport)
                    .ThenInclude(a => a.City)
                .ToListAsync();

            if (!reservations.Any()) return 0;

            var flight = reservations.First().Flight;
            var destination = $"{flight.Destination.FromAirport.City.Name} → {flight.Destination.ToAirport.City.Name}";

            var notifications = reservations.Select(r => new Notification
            {
                UserId = r.UserId,
                FlightId = flightId,
                Type = NotificationType.FlightDelay,
                Message = $"Let {flight.Code} ({destination}) je zakašnjen za {delayMinutes} minuta. Hvala na razumijevanju.",
                SentAt = DateTime.UtcNow,
                IsRead = false,
                IsSystemGenerated = true
            }).ToList();

            _db.Notifications.AddRange(notifications);
            await _db.SaveChangesAsync();

            return notifications.Count;
        }

        // Ostale postojeće metode...
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
                UserFullName = n.User != null ? n.User.FirstName + " " + n.User.LastName : "Sistem",
                UserEmail = n.User != null ? n.User.Email : null,
                FlightId = n.FlightId,
                FlightCode = n.Flight != null ? n.Flight.Code : null,
                Type = n.Type,
                Message = n.Message,
                SentAt = n.SentAt,
                IsRead = n.IsRead,
                AdminReply = n.AdminReply,
                IsSystemGenerated = n.IsSystemGenerated
            };
        }

        public async Task<NotificationGetDTO> CreateAsync(NotificationAddDTO dto)
        {
            var n = new Notification
            {
                UserId = dto.UserId,
                FlightId = dto.FlightId,
                Type = dto.Type,
                Message = dto.Message,
                SentAt = DateTime.UtcNow,
                IsRead = false,
                IsSystemGenerated = false
            };

            _db.Notifications.Add(n);
            await _db.SaveChangesAsync();

            return await GetByIdAsync(n.Id) ?? throw new Exception("Failed to create notification");
        }

        public async Task<NotificationGetDTO?> UpdateAsync(NotificationUpdateDTO dto)
        {
            var n = await _db.Notifications.FindAsync(dto.Id);
            if (n == null) return null;

            n.IsRead = dto.IsRead;
            await _db.SaveChangesAsync();

            return await GetByIdAsync(n.Id);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var n = await _db.Notifications.FindAsync(id);
            if (n == null) return false;

            _db.Notifications.Remove(n);
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<NotificationGetDTO?> ReplyAsync(NotificationReplyDTO dto)
        {
            var n = await _db.Notifications.FindAsync(dto.Id);
            if (n == null) return null;

            n.AdminReply = dto.Reply;
            n.IsRead = true;
            n.RepliedAt = DateTime.UtcNow;

            await _db.SaveChangesAsync();

            return await GetByIdAsync(n.Id);
        }
    }
}


