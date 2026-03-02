using AutoMapper;
using WebApiNet.Application.DTOs.Vehiculo;
using WebApiNet.Core.Entities;
using WebApiNet.Core.Interfaces;
using WebApiNet.Infrastructure.Repositories.UnitOfWork;

namespace WebApiNet.Application.Services
{
    public class VehiculoService : IVehiculoService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public VehiculoService(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }


        public async Task<VehiculoResponse> CreateVehiculoAsync(VehiculoRequest vehiculoDto)
        {
            var existente = await _unitOfWork.Vehiculo.GetByIdAsync(vehiculoDto.Matricula);

            if (existente != null)
            {
                throw new InvalidOperationException("Ya existe un vehículo con esa matrícula.");
            }

            var nuevoVehiculo = _mapper.Map<Vehiculo>(vehiculoDto);
            await _unitOfWork.Vehiculo.AddAsync(nuevoVehiculo);

            return _mapper.Map<VehiculoResponse>(nuevoVehiculo);
        }

        public Task<bool> DeleteVehiculoAsync(string matricula)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<VehiculoResponse>> GetAllVehiculosAsync()
        {
            throw new NotImplementedException();
        }

        public Task<VehiculoResponse> GetVehiculoByMatriculaAsync(string matricula)
        {
            throw new NotImplementedException();
        }

        public Task<VehiculoResponse> UpdateVehiculoAsync(string matricula, VehiculoUpdateRequest vehiculoDto)
        {
            throw new NotImplementedException();
        }
    }
}

     /*   public async Task<bool> DeleteVehiculoAsync(string matricula)
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
     */