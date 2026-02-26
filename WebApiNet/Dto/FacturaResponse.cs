namespace WebApiNet.Dto
{
    public class FacturaResponse
    {
        public int Id { get; set; }
        public string NumeroDeFactura { get; set; }
        public DateTime FechaDeEmision { get; set; }
        public decimal Total { get; set; }
        public string ClienteDni { get; set; }
        public int AlquilerId { get; set; }
    }
}
