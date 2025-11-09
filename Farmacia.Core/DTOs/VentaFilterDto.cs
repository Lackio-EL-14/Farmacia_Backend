using System;

namespace Farmacia.Core.DTOs
{
    /// <summary>
    /// Parámetros para filtrar / paginar la lista de ventas.
    /// </summary>
    public class VentaFilterDto
    {
        /// <summary>Id del cliente para filtrar (opcional)</summary>
        public int? ClienteId { get; set; }

        /// <summary>Fecha mínima (desde) para filtrar (opcional)</summary>
        public DateTime? FechaDesde { get; set; }

        /// <summary>Fecha máxima (hasta) para filtrar (opcional)</summary>
        public DateTime? FechaHasta { get; set; }

        /// <summary>Número (o parte) de la factura para buscar (opcional)</summary>
        public string? NumeroFactura { get; set; }

        /// <summary>Página (1-based) para paginación</summary>
        public int? Page { get; set; } = 1;

        /// <summary>Tamaño de página para paginación</summary>
        public int? PageSize { get; set; } = 10;

        /// <summary>Campo por el que ordenar (ej. "Fecha" o "Total") — opcional</summary>
        public string? OrderBy { get; set; }

        /// <summary>Dirección del orden: "asc" o "desc" — opcional</summary>
        public string? OrderDir { get; set; }
    }
}
