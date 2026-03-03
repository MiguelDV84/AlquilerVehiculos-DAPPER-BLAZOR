using WebApiNet.Infrastructure.Repositories.Auth;
using WebApiNet.Infrastructure.Repositories.Vehiculos;

namespace WebApiNet.Infrastructure.Repositories.UnitOfWork
{
    public interface IUnitOfWork
    {
        IVehiculoRepository Vehiculo { get; }
        IAuthRepository Auth { get; }
    }
}
