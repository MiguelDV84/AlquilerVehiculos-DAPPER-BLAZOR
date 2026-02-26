namespace WebApiNet.Dto
{
    public class LoginResponse
    {
        public string Token { get; set; }
        public DateTime ExpirationAt { get; set; }
        public UserResponse User { get; set; }
    }
}
