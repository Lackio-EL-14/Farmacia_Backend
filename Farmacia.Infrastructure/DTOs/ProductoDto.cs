

namespace Farmacia.Infrastructure.DTOs
{
    public class ProductoDto
    {
        public int Id { get; set; }

        public string Nombre { get; set; } = null!;

        public string? Descripcion { get; set; }

        public decimal PrecioVenta { get; set; }

        public decimal PrecioCompra { get; set; }

        public int StockTotal { get; set; }

        public int CategoriaId { get; set; }

        public int MarcaId { get; set; }

        public int UnidadId { get; set; }

        public int TipoProductoId { get; set; }
        public int SucursalId { get; set; }

    }
}
