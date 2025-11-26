namespace FlightSystem.DTOs.User
{
    public class UserAddDTO
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }   // plaintext → service pravi hash
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Role { get; set; } = "Customer";
    }

}
