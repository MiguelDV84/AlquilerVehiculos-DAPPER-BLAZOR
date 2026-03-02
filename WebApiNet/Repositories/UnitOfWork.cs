namespace WebApiNet.Repositories
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
