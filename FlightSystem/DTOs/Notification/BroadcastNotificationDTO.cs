namespace FlightSystem.DTOs.Notification
{
    public class BroadcastNotificationDTO
    {
        public string Message { get; set; }
        public int? FlightId { get; set; } // Opciono - ako se odnosi na let
    }
}
