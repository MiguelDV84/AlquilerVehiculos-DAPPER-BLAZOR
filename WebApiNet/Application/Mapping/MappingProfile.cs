using AutoMapper;
using WebApiNet.Application.DTOs.Alquiler;
using WebApiNet.Application.DTOs.Auth;
using WebApiNet.Application.DTOs.Vehiculo;
using WebApiNet.Core.Entities;

namespace WebApiNet.Application.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<VehiculoRequest, Vehiculo>();
            CreateMap<Vehiculo, VehiculoRequest>();
            CreateMap<VehiculoRequest, VehiculoResponse>();
            CreateMap<Vehiculo, VehiculoUpdateRequest>();
            CreateMap<Vehiculo, VehiculoResponse>();
            /*  CreateMap<VehiculoUpdateRequest, Vehiculo>()
                  .ForAllMembers(opts => opts.Condition((src, dest, srcMember) =>
                      srcMember != null &&
                      !(srcMember is string s && string.IsNullOrWhiteSpace(s))
                  ));*/
            CreateMap<VehiculoUpdateRequest, Vehiculo>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));


            CreateMap<RegisterRequest, Cliente>();

            CreateMap<AlquilerRequest, Alquiler>();
            CreateMap<Alquiler, AlquilerResponse>();
            CreateMap<Alquiler, AlquilerFinalizadoResponse>();
        }
    }
}
