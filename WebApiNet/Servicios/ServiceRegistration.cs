using WebApiNet.Repositories;

namespace WebApiNet.Servicios
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
