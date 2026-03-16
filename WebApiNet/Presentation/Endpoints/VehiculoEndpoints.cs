using Microsoft.AspNetCore.Authorization;
using WebApiNet.Core.Interfaces;
using WebApiNet.Presentation.Constants;
using WebApiNet.Shared.DTOs.Common;
using WebApiNet.Shared.DTOs.Vehiculo;
using WebApiNet.Shared.Enums;

namespace WebApiNet.Presentation.Endpoints
{
    public static class VehiculoEndpoints
    {
        public static void MapVehiculoEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/vehiculos")
                .WithTags("Vehículos")
                .RequireAuthorization();

            group.MapPost("/", CreateVehiculo)
                .WithName("CreateVehiculo")
                .RequireAuthorization(new AuthorizeAttribute { Roles = RolesConstantes.Admin });

            group.MapGet("/", GetAllVehiculos)
                .WithName("GetAllVehiculos");

            group.MapGet("/{matricula}", GetVehiculo)
                .WithName("GetVehiculo");

            group.MapDelete("/{matricula}", DeleteVehiculo)
                .WithName("DeleteVehiculo")
                .RequireAuthorization(new AuthorizeAttribute { Roles = RolesConstantes.Admin });

            group.MapPut("/{matricula}", UpdateVehiculo)
                .WithName("UpdateVehiculo")
                .RequireAuthorization(new AuthorizeAttribute { Roles = RolesConstantes.Admin });
        }

        private static async Task<IResult> CreateVehiculo(VehiculoRequest request, IVehiculoService service)
        {
            var result = await service.CreateVehiculoAsync(request);

            return Results.Created($"{result.Matricula}", new ApiResponse<object>
            {
                Success = true,
                Message = "Vehículo creado exitosamente",
                Data = result
            });
        }

        private static async Task<IResult> UpdateVehiculo(string matricula, VehiculoUpdateRequest request, IVehiculoService service)
        {
            var result = await service.UpdateVehiculoAsync(matricula, request);

            return Results.Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Vehículo actualizado exitosamente",
                Data = result
            });
        }

        private static async Task<IResult> GetAllVehiculos(IVehiculoService service, int pageNumber = 1, int pageSize = 10)
        {
            var result = await service.GetAllVehiculosAsync(pageNumber, pageSize);

            return Results.Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Vehículos obtenidos exitosamente",
                Data = result
            });
        }

        private static async Task<IResult> GetVehiculo(IVehiculoService service, string matricula)
        {
            var result = await service.GetVehiculoByMatriculaAsync(matricula);

            return Results.Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Vehículo obtenido correctamente",
                Data = result
            });
        }

        private static async Task<IResult> DeleteVehiculo(IVehiculoService service, string matricula)
        {
            var result = await service.DeleteVehiculoAsync(matricula);

            return Results.Ok(new ApiResponse<object>
            {
                Success = result,
                Message = result ? "Vehículo eliminado correctamente" : "Error al eliminar el vehículo",
                Data = result ? new { Matricula = matricula } : null
            });
        }
    }
}
