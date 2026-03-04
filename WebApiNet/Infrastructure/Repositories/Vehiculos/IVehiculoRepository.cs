using WebApiNet.Infrastructure.Repositories.Base;
using WebApiNet.Core.Entities;
using WebApiNet.Presentation.Paged;

namespace WebApiNet.Infrastructure.Repositories.Vehiculos
{
    public interface IVehiculoRepository : IRepository<Vehiculo, String>
    {
    }
}
