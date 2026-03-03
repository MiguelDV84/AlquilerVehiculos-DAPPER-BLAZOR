namespace WebApiNet.Shared.DTOs.Auth
{
    public class UpdateUserRequest
    {
        public string Nombre { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
