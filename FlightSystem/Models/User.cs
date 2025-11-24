namespace FlightSystem.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;

        public string Role { get; set; } = "CLIENT";  // ADMIN / CLIENT
        public bool IsActive { get; set; } = true;

        public ICollection<Reservation>? Reservations { get; set; }
    }
}
