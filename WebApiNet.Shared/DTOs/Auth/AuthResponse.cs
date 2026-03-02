namespace WebApiNet.Application.DTOs.Auth
{
    public class AuthResponse
    {
        public string Token { get; set; }
        public DateTime ExpirationAt { get; set; }
        public UserResponse User { get; set; }
    }
}
