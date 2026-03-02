using WebApiNet.Infrastructure.Repositories.Vehiculos;

namespace WebApiNet.Infrastructure.Repositories.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        public UnitOfWork(IVehiculoRepository vehiculoRepository)
        {
            Vehiculo = vehiculoRepository;
        }
        public IVehiculoRepository Vehiculo { get; }

    }
}
