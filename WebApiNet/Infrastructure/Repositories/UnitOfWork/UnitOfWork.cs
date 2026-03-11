using WebApiNet.Infrastructure.Data;
using WebApiNet.Infrastructure.Repositories.AlquilerRepo;
using WebApiNet.Infrastructure.Repositories.Auth;
using WebApiNet.Infrastructure.Repositories.Vehiculos;

namespace WebApiNet.Infrastructure.Repositories.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DapperContext _context;
        private IVehiculoRepository? _vehiculoRepository;
        private IAuthRepository? _authRepository;
        private IAlquilerRepository? _alquilerRepository;

        public UnitOfWork(DapperContext context)
        {
            _context = context;
        }

        public IVehiculoRepository Vehiculo =>
            _vehiculoRepository ??= new VehiculoRepository(_context);

        public IAuthRepository Auth =>
            _authRepository ??= new AuthRepository(_context);

        public IAlquilerRepository Alquiler =>
            _alquilerRepository ??= new AlquilerRepository(_context);
    }
}
