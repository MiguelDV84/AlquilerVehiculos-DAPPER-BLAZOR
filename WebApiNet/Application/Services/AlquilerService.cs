using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using WebApiNet.Core.Entities;
using WebApiNet.Core.Interfaces;
using WebApiNet.Infrastructure.Data;
using WebApiNet.Shared.DTOs.Alquiler;
using WebApiNet.Shared.Enums;

namespace WebApiNet.Application.Services
{
    public class AlquilerService : IAlquilerService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AlquilerService(ApplicationDbContext context, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<AlquilerResponse> CreateAlquilerAsync(AlquilerRequest request)
        {
            var dni = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value ??
                throw new KeyNotFoundException("No se ha encontrado DNI.");

            var vehiculo = await _context.Vehiculos.FindAsync(request.VehiculoMatricula) ??
                throw new KeyNotFoundException("No existe un vehiculo para esa matricula");

            if (vehiculo.Estado != EstadoVehiculo.Disponible)
            {
                throw new InvalidOperationException("El vehiculo no esta disponible para alquiler");
            }

            var alquiler = _mapper.Map<Alquiler>(request);

            alquiler.ClienteDni = dni;
            alquiler.Precio = vehiculo.Precio;
            alquiler.FechaAlquiler = DateOnly.FromDateTime(DateTime.UtcNow);
            vehiculo.Estado = EstadoVehiculo.Alquilado;

            _context.Alquileres.Add(alquiler);
            await _context.SaveChangesAsync();


            return new AlquilerResponse {
                FechaAlquiler = alquiler.FechaAlquiler,
                FechaDevolucionPrevista = alquiler.FechaDevolucionPrevista,
                Precio = alquiler.Precio,
                ClienteDni = alquiler.ClienteDni,
                VehiculoMatricula = alquiler.VehiculoMatricula
            };
        }

        public async Task<IEnumerable<AlquilerResponse>> GetAllAlquileresAsync()
        {
            var dni = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value ??
                throw new KeyNotFoundException("No se ha encontrado DNI.");
            
            var alquiler = await _context.Alquileres
                .Where(a => a.ClienteDni == dni)
                .ToListAsync();

            var response = _mapper.Map<IEnumerable<AlquilerResponse>>(alquiler);

            return response;
        }

        public async Task<AlquilerResponse> GetAlquilerDtoAsync(string vehiculoMatricula)
        {
            var dni = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value ??
                throw new KeyNotFoundException("No se ha encontrado DNI.");

            var alquiler = await _context.Alquileres
                .Where(a => a.ClienteDni == dni && a.VehiculoMatricula == vehiculoMatricula)
                .FirstAsync() ??
                throw new KeyNotFoundException("No hay alquileres para el cliente");

            var response = _mapper.Map<AlquilerResponse>(alquiler);

            return response;
        }

        public async Task<AlquilerFinalizadoResponse> FinishAlquilerAsync(string vehiculoMatricula)
        {
            var dni = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value ??
                throw new UnauthorizedAccessException("No se ha encontrado DNI en el token.");

            var alquiler = await _context.Alquileres
                .Where(a => a.ClienteDni == dni
                         && a.VehiculoMatricula == vehiculoMatricula
                         && a.FechaDevolucionReal == null)
                .FirstOrDefaultAsync() ??
                throw new KeyNotFoundException("No tienes un alquiler activo para este vehículo.");

            var vehiculo = await _context.Vehiculos.FindAsync(vehiculoMatricula) ??
                throw new KeyNotFoundException("Vehículo no encontrado.");

            vehiculo.Estado = EstadoVehiculo.Disponible;
            alquiler.FechaDevolucionReal = DateOnly.FromDateTime(DateTime.UtcNow);

            await _context.SaveChangesAsync();

            var response = _mapper.Map<AlquilerFinalizadoResponse>(alquiler);

            return response;
        }
    }
}
