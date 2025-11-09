using System;

namespace Farmacia.Core.DTOs
{
    /// <summary>
    /// Parámetros para filtrar / paginar facturas.
    /// </summary>
    public class FacturaFilterDto
    {
        /// <summary>Id de la venta asociada (opcional)</summary>
        public int? VentaId { get; set; }

        /// <summary>Número de factura o parte del mismo (opcional)</summary>
        public string? Numero { get; set; }

        /// <summary>Fecha mínima de emisión (opcional)</summary>
        public DateTime? FechaDesde { get; set; }

        /// <summary>Fecha máxima de emisión (opcional)</summary>
        public DateTime? FechaHasta { get; set; }

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
