using WebApiNet.Core.Enums;

namespace WebApiNet.Application.DTOs.Vehiculo
{
    public class VehiculoUpdateRequest
    {
        public TipoVehiculo? TipoVehiculo { get; set; }

        public int? Kilometraje { get; set; }

        public string? Marca { get; set; }

        public string? Modelo { get; set; }

        public decimal? Precio { get; set; }

        public double? LitrosTanque { get; set; }
    }
}
