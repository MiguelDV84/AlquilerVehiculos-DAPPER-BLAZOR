using AutoMapper;
using WebApiNet.Core.Entities;
using WebApiNet.Core.Exceptions;
using WebApiNet.Core.Interfaces;
using WebApiNet.Infrastructure.Repositories.UnitOfWork;
using WebApiNet.Shared.DTOs.Vehiculo;
using WebApiNet.Shared.Paged;

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
                throw new DuplicateEntityException("Ya existe un vehículo con esa matrícula.");
            }

            var nuevoVehiculo = _mapper.Map<Vehiculo>(vehiculoDto);
            await _unitOfWork.Vehiculo.AddAsync(nuevoVehiculo);

            return _mapper.Map<VehiculoResponse>(nuevoVehiculo);
        }

        public async Task<bool> DeleteVehiculoAsync(string matricula)
        {
            var vehiculo = await _unitOfWork.Vehiculo.GetByIdAsync(matricula);

            if (vehiculo == null)
            {
                throw new NotFoundException($"No existe vehiculo con matricula: {matricula}");
            }

            var succes = await _unitOfWork.Vehiculo.DeleteAsync(matricula);

            return succes;

        }

        public async Task<PagedResult<VehiculoResponse>> GetAllVehiculosAsync(int pageNumber, int pageSize)
        {
            var pagedEntities = await _unitOfWork.Vehiculo.GetAllAsync(pageNumber, pageSize);

            var pagedResult = new PagedResult<VehiculoResponse>
            {
                Items = _mapper.Map<IEnumerable<VehiculoResponse>>(pagedEntities.Items),
                TotalCount = pagedEntities.TotalCount,
                PageNumber = pagedEntities.PageNumber,
                PageSize = pagedEntities.PageSize
            };

            return pagedResult;
        }

        public async Task<VehiculoResponse> GetVehiculoByMatriculaAsync(string matricula)
        {
            var vehiculo = await _unitOfWork.Vehiculo.GetByIdAsync(matricula);

            if(vehiculo == null)
            {
                throw new NotFoundException($"No existe vehiculo con matricula: {matricula}");
            }

            return _mapper.Map<VehiculoResponse>(vehiculo);
        }

        public async Task<VehiculoResponse> UpdateVehiculoAsync(string matricula, VehiculoUpdateRequest vehiculoDto)
        {
            var vehiculo = await _unitOfWork.Vehiculo.GetByIdAsync(matricula);

            if (vehiculo == null)
            {
                throw new NotFoundException($"No existe vehiculo con matricula: {matricula}");
            }

            _mapper.Map(vehiculoDto, vehiculo);
            await _unitOfWork.Vehiculo.UpdateAsync(matricula, vehiculo);

            return _mapper.Map<VehiculoResponse>(vehiculo);
        }
    }
}