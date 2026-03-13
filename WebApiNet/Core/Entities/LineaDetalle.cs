namespace WebApiNet.Core.Entities
{
    public class LineaDetalle
    {
        public int Id { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal Subtotal { get; set; }
        public int ProductoId { get; set; }
        public Alquiler Alquiler { get; set; }
        public int FacturaId { get; set; }
        public Factura Factura { get; set; }
    }
}
