using WebApiNet.Application.DTOs.Auth;
using WebApiNet.Application.DTOs.Common;
using WebApiNet.Core.Interfaces;

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
    }
}