namespace FlightSystem.DTOs.User
{
    public class UserUpdateDTO
    {
        public int Id { get; set; }

        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }

        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public string? Role { get; set; }
    }

}
