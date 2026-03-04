
using WebApiNet.Core.Entities;
using WebApiNet.Infrastructure.Repositories.Base;
using WebApiNet.Presentation.Paged;

namespace WebApiNet.Infrastructure.Repositories.AlquilerRepo
{
    public interface IAlquilerRepository : IRepository<Alquiler, int>
    {
        Task<PagedResult<Alquiler>> GetPagedAsync(int pageNumber, int pageSize);
    }
}
