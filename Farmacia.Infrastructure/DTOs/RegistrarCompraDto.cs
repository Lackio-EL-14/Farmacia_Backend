namespace Farmacia.Infrastructure.DTOs
{
    public class RegistrarCompraDto
    {
        public int ProveedorId { get; set; }
        public List<DetalleCompraCrearDto> Detalles { get; set; } = new();
    }

    public class DetalleCompraCrearDto
    {
        public int ProductoId { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
    }
}
