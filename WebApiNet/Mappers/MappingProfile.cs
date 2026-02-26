using AutoMapper;
using WebApiNet.Dto;
using WebApiNet.Models;

namespace WebApiNet.Mappers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<VehiculoRequest, Vehiculos>();
            CreateMap<Vehiculos, VehiculoRequest>();
            CreateMap<VehiculoRequest, VehiculoResponse>();
            CreateMap<Vehiculos, VehiculoResponse>();
            CreateMap<VehiculoUpdateRequest, Vehiculos>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) =>
                    srcMember != null &&
                    !(srcMember is string s && string.IsNullOrWhiteSpace(s))
                ));

            CreateMap<RegisterRequest, Cliente>();

            CreateMap<AlquilerRequest, Alquiler>();
            CreateMap<Alquiler, AlquilerResponse>();
            CreateMap<Alquiler, AlquilerFinalizadoResponse>();
        }
    }
}
