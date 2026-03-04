using WebApiNet.Core.Interfaces;
using WebApiNet.Shared.DTOs.Auth;
using WebApiNet.Shared.DTOs.Common;

namespace WebApiNet.Presentation.Endpoints
{
    public static class AuthEndpoints
    {
        public static void MapAuthEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/auth")
                .WithTags("Autenticación");

            group.MapPost("/login", Login)
                .WithName("Login");

            group.MapPost("/register", Register)
                .WithName("Register");

            group.MapGet("/user", GetUser)
                .WithName("GetUser")
                .RequireAuthorization();

            group.MapGet("/users", GetAllUser)
                .WithName("GetAllUser")
                .RequireAuthorization();

            group.MapDelete("/user", DeleteUser)
                .WithName("DeleteUser")
                .RequireAuthorization();

            group.MapPut("/user", UpdateUser)
                .WithName("UpdateUser")
                .RequireAuthorization();
        }

        private static async Task<IResult> Login(LoginRequest request, IAuthService service)
        {
            var result = await service.Login(request);
            if (result == null)
            {
                return Results.Unauthorized();
            }
            return Results.Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Inicio de sesión exitoso",
                Data = result
            });
        }

        private static async Task<IResult> Register(RegisterRequest request, IAuthService service)
        {
            var result = await service.Register(request);
            if (result == null)
            {
                return Results.BadRequest(new ApiResponse<object>
                {
                    Success = false,
                    Message = "Error al registrar el usuario",
                    Data = null
                });
            }
            return Results.Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Usuario registrado exitosamente",
                Data = result
            });
        }

        public static async Task<IResult> GetAllUser(IAuthService service, int pageNumber = 1, int pageSize = 5)
        {
            var result = await service.GetAllUserAync(pageNumber, pageSize);
            return Results.Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Usuarios obtenidos exitosamente",
                Data = result
            });
        }

        private static async Task<IResult> DeleteUser(IAuthService service)
        {
            var result = await service.DeleteUserAsync();
            if (!result)
            {
                return Results.BadRequest(new ApiResponse<object>
                {
                    Success = false,
                    Message = "Error al eliminar el usuario",
                    Data = null
                });
            }
            return Results.Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Usuario eliminado exitosamente",
                Data = null
            });
        }

        private static async Task<IResult> UpdateUser(UpdateUserRequest request, IAuthService service)
        {
            var result = await service.UpdateUserAsync(request);
            if (result == null)
            {
                return Results.BadRequest(new ApiResponse<object>
                {
                    Success = false,
                    Message = "Error al actualizar el usuario",
                    Data = null
                });
            }
            return Results.Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Usuario actualizado exitosamente",
                Data = result
            });
        }

        private static async Task<IResult> GetUser(IAuthService service)
        {
            var result = await service.GetUserAsync();
            if (result == null)
            {
                return Results.NotFound(new ApiResponse<object>
                {
                    Success = false,
                    Message = "Usuario no encontrado",
                    Data = null
                });
            }
            return Results.Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Usuario obtenido exitosamente",
                Data = result
            });
        }
    }
}