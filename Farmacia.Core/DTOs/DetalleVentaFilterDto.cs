namespace Farmacia.Core.DTOs
{
    /// <summary>
    /// Parámetros para filtrar / paginar detalles de venta.
    /// </summary>
    public class DetalleVentaFilterDto
    {
        /// <summary>Id de la venta (filtrar por una venta específica)</summary>
        public int? VentaId { get; set; }

        /// <summary>Id del producto (filtrar por producto)</summary>
        public int? ProductoId { get; set; }

        /// <summary>Búsqueda por nombre de producto (parcial)</summary>
        public string? ProductoNombre { get; set; }

        /// <summary>Página (1-based) para paginación</summary>
        public int? Page { get; set; } = 1;

        /// <summary>Tamaño de página para paginación</summary>
        public int? PageSize { get; set; } = 10;

        /// <summary>Orden (campo)</summary>
        public string? OrderBy { get; set; }

        /// <summary>Dirección del orden ("asc" | "desc")</summary>
        public string? OrderDir { get; set; }
    }
}
