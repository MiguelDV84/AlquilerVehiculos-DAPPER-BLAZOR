using WebApiNet.Infrastructure.Repositories.Base;
using WebApiNet.Core.Entities;
using WebApiNet.Shared.Paged;

namespace WebApiNet.Infrastructure.Repositories.Vehiculos
{
    public interface IVehiculoRepository : IRepository<Vehiculo, String>
    {
    }
}
