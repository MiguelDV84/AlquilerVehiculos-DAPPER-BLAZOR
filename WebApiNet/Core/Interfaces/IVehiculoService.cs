

using WebApiNet.Presentation.Paged;
using WebApiNet.Shared.DTOs.Vehiculo;

namespace WebApiNet.Core.Interfaces
{
    public interface IVehiculoService
    {
        Task<PagedResult<VehiculoResponse>> GetAllVehiculosAsync(int pageNumber, int pageSize);
        Task<VehiculoResponse> GetVehiculoByMatriculaAsync(string matricula);
        Task<VehiculoResponse> CreateVehiculoAsync(VehiculoRequest vehiculoDto);
        Task<VehiculoResponse> UpdateVehiculoAsync(string matricula, VehiculoUpdateRequest vehiculoDto);
        Task<bool> DeleteVehiculoAsync(string matricula);
    }
}
