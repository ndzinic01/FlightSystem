using FlightSystem.DTOs.User;

namespace FlightSystem.DTOs.Login
{
    public class LoginResponseDTO
    {
        public string Token { get; set; }
        public UserGetDTO User { get; set; }
    }
}
