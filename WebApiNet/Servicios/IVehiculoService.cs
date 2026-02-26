using WebApiNet.Dto;
using WebApiNet.Models;
using WebApiNet.Tipos;

namespace WebApiNet.Servicios
{
    public interface IVehiculoService
    {
        Task<IEnumerable<VehiculoResponse>> GetAllVehiculosAsync();
        Task<VehiculoResponse> GetVehiculoByMatriculaAsync(string matricula);
        Task<VehiculoResponse> CreateVehiculoAsync(VehiculoRequest vehiculoDto);
        Task<VehiculoResponse> UpdateVehiculoAsync(string matricula, VehiculoUpdateRequest vehiculoDto);
        Task<bool> DeleteVehiculoAsync(string matricula);
        Task<bool> CambiarEstadoVehiculoAsync(string matricula, EstadoVehiculo nuevoEstado);
    }
}
