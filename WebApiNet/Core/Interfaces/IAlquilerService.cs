
using WebApiNet.Application.DTOs.Alquiler;

namespace WebApiNet.Core.Interfaces
{
    public interface IAlquilerService
    {
        Task<AlquilerResponse> GetAlquilerDtoAsync(string vehiculoMatricula);
        Task<AlquilerResponse> CreateAlquilerAsync(AlquilerRequest request);
        Task<IEnumerable<AlquilerResponse>> GetAllAlquileresAsync();
        Task<AlquilerFinalizadoResponse> FinishAlquilerAsync(string vehiculoMatricula);
    }
}
