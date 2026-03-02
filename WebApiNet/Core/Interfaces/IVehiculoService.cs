using WebApiNet.Application.DTOs.Vehiculo;


namespace WebApiNet.Core.Interfaces
{
    public interface IVehiculoService
    {
        Task<IEnumerable<VehiculoResponse>> GetAllVehiculosAsync();
        Task<VehiculoResponse> GetVehiculoByMatriculaAsync(string matricula);
        Task<VehiculoResponse> CreateVehiculoAsync(VehiculoRequest vehiculoDto);
        Task<VehiculoResponse> UpdateVehiculoAsync(string matricula, VehiculoUpdateRequest vehiculoDto);
        Task<bool> DeleteVehiculoAsync(string matricula);
    }
}
