using WebApiNet.Application.Mapping;
using WebApiNet.Application.Services;
using WebApiNet.Core.Interfaces;
using WebApiNet.Infrastructure.Data;
using WebApiNet.Infrastructure.Repositories.Auth;
using WebApiNet.Infrastructure.Repositories.UnitOfWork;
using WebApiNet.Infrastructure.Repositories.Vehiculos;

namespace WebApiNet.Applicacion.DependencyInjection
{
    public static class ServiceRegistration
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddAutoMapper(cfg => cfg.AddProfile<MappingProfile>());

            // ✅ Application Services
            services.AddScoped<IVehiculoService, VehiculoService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IAlquilerService, AlquilerService>();

            return services;
        }
    }
}
