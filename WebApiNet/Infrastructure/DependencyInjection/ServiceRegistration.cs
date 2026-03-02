using WebApiNet.Infrastructure.Repositories.UnitOfWork;
using WebApiNet.Infrastructure.Repositories.Vehiculos;

namespace WebApiNet.Infrastructure.DependencyInjection
{
    public static class ServiceRegistration
    {
        public static void AddInfrastructure(this IServiceCollection services)
        {
            services.AddTransient<IVehiculoRepository, VehiculoRepository>();
            services.AddTransient<IUnitOfWork, UnitOfWork>();
        }
    }
}
