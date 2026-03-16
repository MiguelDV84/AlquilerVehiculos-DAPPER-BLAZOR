namespace WebApiNet.Shared.Enums
{
    public enum EstadoFactura
    {
        Borrador = 1,      // Editando, sin validez legal aún.
        Emitida = 2,        // Documento finalizado y enviado al cliente.
        Pagada = 3,         // El cobro se ha realizado por completo.
        Parcial = 4,        // Se ha recibido un pago pero queda saldo pendiente.
        Vencida = 5,        // Pasó la fecha de vencimiento sin completarse el pago.
        Anulada = 6,        // Cancelada (normalmente requiere factura rectificativa).
        Rechazada = 7       // El cliente ha rechazado la factura por algún error.
    }
}
