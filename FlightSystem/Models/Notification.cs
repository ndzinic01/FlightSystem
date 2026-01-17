using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FlightSystem.Models
{
    public enum NotificationType
    {
        UserInquiry = 0,        // Korisnički upit
        FlightDelay = 1,        // Zakašnjenje leta
        FlightCancellation = 2, // Otkazan let
        FlightReschedule = 3,   // Promjena vremena
        SystemBroadcast = 4     // Opšta obavijest
    }

    public class Notification
    {
        [Key]
        public int Id { get; set; }

        public int? UserId { get; set; } // 🔥 NULL za broadcast poruke
        [ForeignKey(nameof(UserId))]
        public User? User { get; set; }

        public int? FlightId { get; set; } // 🔥 NULL za opšte obavijesti
        [ForeignKey(nameof(FlightId))]
        public Flight? Flight { get; set; }

        public NotificationType Type { get; set; } // 🔥 NOVO - tip notifikacije

        public string Message { get; set; }
        public DateTime SentAt { get; set; } = DateTime.UtcNow;
        public bool IsRead { get; set; } = false;
        public string? AdminReply { get; set; }
        public DateTime? RepliedAt { get; set; }

        public bool IsSystemGenerated { get; set; } = false; // 🔥 NOVO - da li je auto generisano
    }
}
