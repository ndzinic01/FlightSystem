using FlightSystem.Models;

namespace FlightSystem.DTOs.Notification
{
    public class NotificationGetDTO
    {
        public int Id { get; set; }
        public int? UserId { get; set; }
        public string? UserFullName { get; set; }
        public string? UserEmail { get; set; }
        public int? FlightId { get; set; }
        public string? FlightCode { get; set; }
        public NotificationType Type { get; set; } // 🔥 NOVO
        public string Message { get; set; }
        public DateTime SentAt { get; set; }
        public bool IsRead { get; set; }
        public string? AdminReply { get; set; }
        public bool IsSystemGenerated { get; set; } // 🔥 NOVO
    }

}
