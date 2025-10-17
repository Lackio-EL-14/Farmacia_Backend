

namespace Farmacia.Infrastructure.DTOs
{
    public class VentaDto
    {
        public int Id { get; set; }
        public DateTime FechaHora { get; set; } = DateTime.Now;
        public decimal Total { get; set; }

        public int ClienteId { get; set; }

        public List<DetalleVentaDto> Detalles { get; set; } = new List<DetalleVentaDto>();
    }
}
