using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WebApiNet.Data;
using WebApiNet.Dto;
using WebApiNet.Models;
using WebApiNet.Tipos;
using System.Security.Claims;

namespace WebApiNet.Servicios
{
    public class VehiculoService : IVehiculoService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public VehiculoService(ApplicationDbContext context, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<bool> CambiarEstadoVehiculoAsync(string matricula, EstadoVehiculo nuevoEstado)
        {
            throw new NotImplementedException();
        }

        public async Task<VehiculoResponse> CreateVehiculoAsync(VehiculoRequest vehiculoDto)
        {
            if (_context.Vehiculos.Any(v => v.Matricula == vehiculoDto.Matricula))
            {
                throw new InvalidOperationException("Ya existe un vehículo con esa matrícula.");
            }

            var vehiculo = _mapper.Map<Vehiculos>(vehiculoDto);

            _context.Vehiculos.Add(vehiculo);
            await _context.SaveChangesAsync();

            var response = _mapper.Map<VehiculoResponse>(vehiculoDto);

            return response;
        }

        public async Task<bool> DeleteVehiculoAsync(string matricula)
        {
            try
            {
                var vehiculo = await _context.Vehiculos.FindAsync(matricula) ??
                throw new KeyNotFoundException("Vehículo no encontrado.");

                _context.Vehiculos.Remove(vehiculo);
                await _context.SaveChangesAsync();

                return true;
            } catch(DbUpdateException)
            {
                throw new InvalidOperationException("No se puede eliminar el vehículo porque tiene alquileres asociados.");
            }
            
        }

        public async Task<IEnumerable<VehiculoResponse>> GetAllVehiculosAsync()
        {
            var dni = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var vehiculos = await _context.Vehiculos
                .Include(v => v.Alquileres.Where(a => a.ClienteDni == dni))
                .ToListAsync();
            var response = _mapper.Map<IEnumerable<VehiculoResponse>>(vehiculos);

            return response;
        }

        public async Task<VehiculoResponse> GetVehiculoByMatriculaAsync(string matricula)
        {
            var dni = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            Console.WriteLine($"DNI del usuario autenticado: {dni}");
            var vehiculo = await _context.Vehiculos
                .Include(v => v.Alquileres.Where(a => a.ClienteDni == dni ))
                .FirstOrDefaultAsync(v => v.Matricula == matricula) ??
                throw new KeyNotFoundException("Vehículo no encontrado.");

            var response = _mapper.Map<VehiculoResponse>(vehiculo);
            return response;
        }

        public async Task<VehiculoResponse> UpdateVehiculoAsync(string matricula, VehiculoUpdateRequest vehiculoRequest)
        {
            var vehiculo = _context.Vehiculos.Find(matricula) ??
                throw new KeyNotFoundException("Vehículo no encontrado.");

            _mapper.Map(vehiculoRequest, vehiculo);
            await _context.SaveChangesAsync();

            var response = _mapper.Map<VehiculoResponse>(vehiculo);

            return response;
        }

    }
}
