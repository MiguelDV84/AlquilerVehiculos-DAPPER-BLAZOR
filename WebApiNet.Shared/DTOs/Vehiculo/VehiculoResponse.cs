
using WebApiNet.Shared.DTOs.Alquiler;

namespace WebApiNet.Shared.DTOs.Vehiculo
{
    public class VehiculoResponse
    {
        public string Matricula { get; set; }
        public int TipoVehiculo { get; set; }
        public int Kilometraje { get; set; }
        public string Marca { get; set; }
        public string Modelo { get; set; }
        public decimal Precio { get; set; }
        public double LitrosTanque { get; set; }
        public int Estado { get; set; }
        public List<AlquilerResponse> Alquileres { get; set; } = new();
    }
}
