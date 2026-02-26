using WebApiNet.Dto;

namespace WebApiNet.Servicios
{
    public interface IAuthService
    {
        Task<UserResponse> Register(RegisterRequest registerDto);
        Task<LoginResponse> Login(LoginRequest loginDto);
        Task<UserResponse> GetUserAsync();
        Task<IEnumerable<UserResponse>> GetAllUserAync();
    }
}
