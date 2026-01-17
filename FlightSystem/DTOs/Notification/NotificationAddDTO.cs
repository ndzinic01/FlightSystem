using FlightSystem.Models;

namespace FlightSystem.DTOs.Notification
{
    public class NotificationAddDTO
    {
        public int? UserId { get; set; } // NULL za broadcast
        public int? FlightId { get; set; } // NULL za opšte
        public NotificationType Type { get; set; } = NotificationType.UserInquiry;
        public string Message { get; set; }
    }
}
