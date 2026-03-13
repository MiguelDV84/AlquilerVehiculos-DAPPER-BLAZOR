using WebApiNet.Shared.Enums;

namespace WebApiNet.Core.Entities
{
    public class Factura
    {
        public int Id { get; set; }
        public string NumeroFactura { get; set; }
        public DateTime FechaEmision { get; set; }
        public DateTime FechaVencimiento { get; set; }
        public decimal BaseImponible { get; set; }
        public decimal PorcentajeIVA { get; set; }
        public decimal CuotaIVA { get; set; }
        public decimal Total { get; set; }
        public EstadoFactura Estado { get; set; }
        public string Observaciones { get; set; }

        public Cliente Cliente { get; set; }
        public int ClienteId { get; set; }
        public Alquiler Alquiler { get; set; }
        public int AlquilerId { get; set; }
        public ICollection<LineaDetalle> LineasDetalle { get; set; } = new List<LineaDetalle>();

        public decimal CalcularTotal => BaseImponible + (BaseImponible * (PorcentajeIVA / 100));
    }
}
