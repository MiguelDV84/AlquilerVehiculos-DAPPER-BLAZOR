
using WebApiNet.Core.Entities;
using WebApiNet.Infrastructure.Repositories.Base;
using WebApiNet.Shared.Paged;

namespace WebApiNet.Infrastructure.Repositories.AlquilerRepo
{
    public interface IAlquilerRepository : IRepository<Alquiler, int>
    {
        Task<PagedResult<Alquiler>> GetPagedAsync(int pageNumber, int pageSize);
        Task<IEnumerable<Alquiler>> GetAllByDniAsync(string dni);
        Task<Alquiler?> GetActiveByDniAndMatriculaAsync(string dni, string matricula);
        Task<Alquiler> FinalizarAlquilerAsync(int id, DateOnly fechaDevolucionReal);
    }
}
