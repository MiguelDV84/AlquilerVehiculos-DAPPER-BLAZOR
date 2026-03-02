using WebApiNet.Application.DTOs.Common;
using WebApiNet.Application.DTOs.Vehiculo;
using WebApiNet.Core.Interfaces;

namespace WebApiNet.Presentation.Endpoints
{
    public static class VehiculoEndpoints
    {
        public static void MapVehiculoEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapPost("/api/v2/vehiculos", CreateVehiculo);
        }

        private static async Task<IResult> CreateVehiculo(VehiculoRequest request, IVehiculoService service)
        {
            var result = await service.CreateVehiculoAsync(request);

            return Results.Created($"/api/v2/vehiculos/{result.Matricula}", new ApiResponse<object>
            {
                Success = true,
                Message = "Vehículo creado exitosamente",
                Data = result
            });
        }
    }
}
