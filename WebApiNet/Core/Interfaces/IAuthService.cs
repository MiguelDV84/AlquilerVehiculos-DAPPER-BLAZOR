using WebApiNet.Shared.DTOs.Auth;

namespace WebApiNet.Core.Interfaces
{
    public interface IAuthService
    {
        Task<UserResponse> Register(RegisterRequest registerDto);
        Task<AuthResponse> Login(LoginRequest loginDto);
        Task<UserResponse> GetUserAsync();
        Task<IEnumerable<UserResponse>> GetAllUserAync();
        Task<UserResponse> UpdateUserAsync(string id, UpdateUserRequest updateUserDto);
         Task<bool> DeleteUserAsync(string id);
    }
}
