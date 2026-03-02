namespace WebApiNet.Application.DTOs.Alquiler
{
    public class AlquilerResponse
    {
        public DateOnly FechaAlquiler { get; set; }
        public DateOnly FechaDevolucionPrevista { get; set; }
        public DateOnly? FechaDevolucionReal { get; set; }
        public decimal Precio { get; set; }
        public string ClienteDni { get; set; }
        public string VehiculoMatricula { get; set; }
    }

    public class AlquilerFinalizadoResponse
    {
        public DateOnly FechaAlquiler { get; set; }
        public DateOnly FechaDevolucionPrevista { get; set; }
        public DateOnly FechaDevolucionReal { get; set; }
        public decimal Precio { get; set; }
        public string ClienteDni { get; set; }
        public string VehiculoMatricula { get; set; }
    }
}
