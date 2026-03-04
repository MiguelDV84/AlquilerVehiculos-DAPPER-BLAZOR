using WebApiNet.Application.Services;
using WebApiNet.Core.Interfaces;
using WebApiNet.Infrastructure.Data;
using WebApiNet.Infrastructure.Repositories.Auth;
using WebApiNet.Infrastructure.Repositories.UnitOfWork;
using WebApiNet.Infrastructure.Repositories.Vehiculos;

namespace WebApiNet.Infrastructure.DependencyInjection
{
    public static class ServiceRegistration
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddSingleton<DapperContext>();
            services.AddTransient<IVehiculoRepository, VehiculoRepository>();

            services.AddTransient<IAuthRepository, AuthRepository>();
            services.AddTransient<IAuthService, AuthService>();

            services.AddTransient<IUnitOfWork, UnitOfWork>();

            return services;
        }
    }
}
