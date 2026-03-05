using WebApiNet.Shared.DTOs.Auth;
using WebApiNet.Shared.Paged;

namespace WebApiNet.Core.Interfaces
{
    public interface IAuthService
    {
        Task<UserResponse> Register(RegisterRequest registerDto);
        Task<AuthResponse> Login(LoginRequest loginDto);
        Task<UserResponse> GetUserAsync();
        Task<PagedResult<UserResponse>> GetAllUserAync(int pageNumber, int pageSize);
        Task<UserResponse> UpdateUserAsync(UpdateUserRequest updateUserDto);
         Task<bool> DeleteUserAsync();
    }
}
