using WebApiNet.Shared.Enums;

namespace WebApiNet.Shared.DTOs.Vehiculo
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
