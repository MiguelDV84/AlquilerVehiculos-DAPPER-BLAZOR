using WebApiNet.Infrastructure.Repositories.Paged;
using WebApiNet.Shared.DTOs.Auth;

namespace WebApiNet.Core.Interfaces
{
    public interface IAuthService
    {
        Task<UserResponse> Register(RegisterRequest registerDto);
        Task<AuthResponse> Login(LoginRequest loginDto);
        Task<UserResponse> GetUserAsync();
        Task<PagedResult<UserResponse>> GetAllUserAync(int pageNumber, int pageSize);
        Task<UserResponse> UpdateUserAsync(string id, UpdateUserRequest updateUserDto);
         Task<bool> DeleteUserAsync(string id);
    }
}
