using WebApiNet.Core.Interfaces;
using WebApiNet.Shared.DTOs.Alquiler;
using WebApiNet.Shared.DTOs.Common;

namespace WebApiNet.Presentation.Endpoints
{
    public static class AlquilerEndpoints
    {
        public static void MapAlquilerEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/alquileres")
                .WithTags("Alquileres")
                .RequireAuthorization();

            group.MapPost("/", CreateAlquiler)
                .WithName("CreateAlquiler");
    
            group.MapGet("/", GetAllAlquileres)
                .WithName("GetAllAlquileres");

            group.MapGet("/{matricula}", GetAlquiler)
                .WithName("GetAlquiler");

            group.MapPut("/finalizar/{matricula}", FinalizarAlquiler)
                .WithName("FinalizarAlquiler");

            group.MapPut("/{matricula}", ActualizarAlquiler)
                .WithName("ActualizarAlquiler");
        }

        private static async Task<IResult> CreateAlquiler(AlquilerRequest request, IAlquilerService service)
        {
            var result = await service.CreateAlquilerAsync(request);

            return Results.Created($"/api/alquileres/{result.VehiculoMatricula}", new ApiResponse<object>
            {
                Success = true,
                Message = "Alquiler creado exitosamente",
                Data = result
            });
        }

        private static async Task<IResult> GetAllAlquileres(IAlquilerService service)
        {
            var result = await service.GetAllAlquileresAsync();

            return Results.Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Alquileres obtenidos exitosamente",
                Data = result
            });
        }

        private static async Task<IResult> GetAlquiler(string matricula, IAlquilerService service)
        {
            var result = await service.GetAlquilerDtoAsync(matricula);

            return Results.Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Alquiler obtenido correctamente",
                Data = result
            });
        }

        private static async Task<IResult> FinalizarAlquiler(string matricula, IAlquilerService service)
        {
            var result = await service.FinishAlquilerAsync(matricula);

            return Results.Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Alquiler finalizado correctamente",
                Data = result
            });
        }

        private static async Task<IResult> ActualizarAlquiler(IAlquilerService service, string matricula, AlquilerUpdateRequest request)
        {
            var result = await service.UpdateAlquilerAsync(matricula, request);

            return Results.Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Alquiler actualizado correctamente",
                Data = result
            });
        }
    }
}
