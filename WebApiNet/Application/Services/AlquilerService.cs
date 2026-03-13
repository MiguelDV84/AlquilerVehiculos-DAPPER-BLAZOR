using AutoMapper;
using System.Security.Claims;
using WebApiNet.Core.Entities;
using WebApiNet.Core.Exceptions;
using WebApiNet.Core.Interfaces;
using WebApiNet.Infrastructure.Repositories.UnitOfWork;
using WebApiNet.Shared.DTOs.Alquiler;
using WebApiNet.Shared.Enums;

namespace WebApiNet.Application.Services
{
    public class AlquilerService : IAlquilerService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AlquilerService(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<AlquilerResponse> CreateAlquilerAsync(AlquilerRequest request)
        {
            var dni = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value ??
                throw new UnauthorizedAccessException("No se ha encontrado DNI en el token.");

            var vehiculo = await _unitOfWork.Vehiculo.GetByIdAsync(request.VehiculoMatricula) ??
                throw new NotFoundException($"No existe un vehículo con la matrícula {request.VehiculoMatricula}.");

            if (vehiculo.Estado != EstadoVehiculo.Disponible)
                throw new InvalidOperationException("El vehículo no está disponible para alquiler.");

            var alquiler = _mapper.Map<Alquiler>(request);
            alquiler.ClienteDni = dni;

            if(request.FechaDevolucionPrevista.CompareTo(DateOnly.FromDateTime(DateTime.UtcNow)) <= 0)
                throw new InvalidOperationException("La fecha de devolución prevista debe ser posterior a la fecha actual.");

            var precioAlquiler = vehiculo.Precio * (request.FechaDevolucionPrevista.DayNumber - DateOnly.FromDateTime(DateTime.UtcNow).DayNumber);
            alquiler.Precio = precioAlquiler;
            alquiler.FechaAlquiler = DateOnly.FromDateTime(DateTime.UtcNow);

            vehiculo.Estado = EstadoVehiculo.Alquilado;

            await _unitOfWork.Alquiler.AddAsync(alquiler);
            await _unitOfWork.Vehiculo.UpdateAsync(vehiculo.Matricula, vehiculo);

            return _mapper.Map<AlquilerResponse>(alquiler);
        }

        public async Task<IEnumerable<AlquilerResponse>> GetAllAlquileresAsync()
        {
            var dni = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value ??
                throw new UnauthorizedAccessException("No se ha encontrado DNI en el token.");

            var alquileres = await _unitOfWork.Alquiler.GetAllByDniAsync(dni);

            return _mapper.Map<IEnumerable<AlquilerResponse>>(alquileres);
        }

        public async Task<AlquilerResponse> GetAlquilerDtoAsync(string vehiculoMatricula)
        {
            var dni = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value ??
                throw new UnauthorizedAccessException("No se ha encontrado DNI en el token.");

            var alquiler = await _unitOfWork.Alquiler.GetActiveByDniAndMatriculaAsync(dni, vehiculoMatricula) ??
                throw new NotFoundException("No se encontró un alquiler activo para ese vehículo.");

            return _mapper.Map<AlquilerResponse>(alquiler);
        }

        public async Task<AlquilerFinalizadoResponse> FinishAlquilerAsync(string vehiculoMatricula)
        {
            var dni = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value ??
                throw new UnauthorizedAccessException("No se ha encontrado DNI en el token.");

            var alquiler = await _unitOfWork.Alquiler.GetActiveByDniAndMatriculaAsync(dni, vehiculoMatricula) ??
                throw new NotFoundException("No tienes un alquiler activo para este vehículo.");

            var vehiculo = await _unitOfWork.Vehiculo.GetByIdAsync(vehiculoMatricula) ??
                throw new NotFoundException($"No existe un vehículo con la matrícula {vehiculoMatricula}.");

            var fechaDevolucion = DateOnly.FromDateTime(DateTime.UtcNow);
            vehiculo.Estado = EstadoVehiculo.Disponible;

            var alquilerFinalizado = await _unitOfWork.Alquiler.FinalizarAlquilerAsync(alquiler.Id, fechaDevolucion);
            await _unitOfWork.Vehiculo.UpdateAsync(vehiculo.Matricula, vehiculo);

            return _mapper.Map<AlquilerFinalizadoResponse>(alquilerFinalizado);
        }

        public async Task<AlquilerResponse> UpdateAlquilerAsync(string vehiculoMatricula, AlquilerUpdateRequest request)
        {
            var dni = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value ??
                throw new UnauthorizedAccessException("No se ha encontrado DNI en el token.");

            var vehiculo =  await _unitOfWork.Vehiculo.GetByIdAsync(vehiculoMatricula) ??
                throw new NotFoundException($"No existe un vehículo con la matrícula {vehiculoMatricula}.");

            var alquiler = await _unitOfWork.Alquiler.GetActiveByDniAndMatriculaAsync(dni, vehiculoMatricula) ??
                throw new NotFoundException("No tienes un alquiler activo para este vehículo.");

            alquiler.FechaDevolucionPrevista = request.FechaDevolucionPrevista;

            var alquilerActualizado = await _unitOfWork.Alquiler.UpdateAsync(alquiler.Id, alquiler);

            return _mapper.Map<AlquilerResponse>(alquilerActualizado);
        }
    }
}
