using WebApiNet.Infrastructure.Data;
using WebApiNet.Infrastructure.Repositories.Auth;
using WebApiNet.Infrastructure.Repositories.Vehiculos;

namespace WebApiNet.Infrastructure.Repositories.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DapperContext _context;
        private IVehiculoRepository? _vehiculoRepository;
        private IAuthRepository? authRepository;

        public UnitOfWork(DapperContext context)
        {
            _context = context;
        }

        public IVehiculoRepository Vehiculo =>
            _vehiculoRepository ??= new VehiculoRepository(_context);

        public IAuthRepository Auth =>
            authRepository ??= new AuthRepository(_context);

    }
}
