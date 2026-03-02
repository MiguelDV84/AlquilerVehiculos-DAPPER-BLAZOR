using WebApiNet.Infrastructure.Data;
using WebApiNet.Infrastructure.Repositories.Vehiculos;

namespace WebApiNet.Infrastructure.Repositories.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DapperContext _context;
        private IVehiculoRepository? _vehiculoRepository;

        public UnitOfWork(DapperContext context)
        {
            _context = context; // 👈 AQUÍ se inyecta
        }

        public IVehiculoRepository Vehiculo =>
            _vehiculoRepository ??= new VehiculoRepository(_context);

    }
}
