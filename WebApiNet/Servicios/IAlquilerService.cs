using WebApiNet.Dto;
using WebApiNet.Tipos;

namespace WebApiNet.Servicios
{
    public interface IAlquilerService
    {
        Task<AlquilerResponse> GetAlquilerDtoAsync(string vehiculoMatricula);
        Task<AlquilerResponse> CreateAlquilerAsync(AlquilerRequest request);
        Task<IEnumerable<AlquilerResponse>> GetAllAlquileresAsync();
        Task<AlquilerFinalizadoResponse> FinishAlquilerAsync(string vehiculoMatricula);
    }
}
