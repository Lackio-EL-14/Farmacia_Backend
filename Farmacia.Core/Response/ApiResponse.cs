
using Farmacia.Core.Entities;
using Farmacia.Core.Pagination;

namespace Farmacia.Core.Response
{
    public class ApiResponse<T>
    {
        private PagedList<DetalleVentaDto> detalles;
        private string v;
        private PaginationMetadata paginationMetadata;
        private List<DetalleVentaDto> detalles1;
        private DetalleVentaDto detalle;
        private List<DetalleVentaDto> detalles2;
        private Venta venta;
        private bool v1;
        private string v2;
        private List<DetalleVentaDto> detalles3;
        private DetalleVentaDto detalle1;

        public bool Success { get; set; }
        public string? Message { get; set; }
        public T? Data { get; set; }
        public PaginationMetadata? Pagination { get; set; }

        public ApiResponse(T? data, bool success = true, string? message = null, PaginationMetadata? pagination = null)
        {
            Success = success;
            Message = message;
            Data = data;
            Pagination = pagination;
        }

        public ApiResponse(PagedList<DetalleVentaDto> detalles, string v, PaginationMetadata paginationMetadata)
        {
            this.detalles = detalles;
            this.v = v;
            this.paginationMetadata = paginationMetadata;
        }

        public ApiResponse(object facturas, List<DetalleVentaDto> detalles1, string v)
        {
            this.detalles1 = detalles1;
            this.v = v;
        }

        public ApiResponse(Entities.Venta venta, DetalleVentaDto detalle, string v)
        {
            this.detalle = detalle;
            this.v = v;
        }

        public ApiResponse(Entities.Venta venta, List<DetalleVentaDto> detalles2, string v)
        {
            this.detalles2 = detalles2;
            this.v = v;
        }

        public ApiResponse(Venta venta, bool v1, string v2)
        {
            this.venta = venta;
            this.v1 = v1;
            this.v2 = v2;
        }

        public ApiResponse(List<DetalleVentaDto> detalles3, string v)
        {
            this.detalles3 = detalles3;
            this.v = v;
        }

        public ApiResponse(DetalleVentaDto detalle1, string v)
        {
            this.detalle1 = detalle1;
            this.v = v;
        }

        public static ApiResponse<T> Fail(string message, int status = 400)
        {
            return new ApiResponse<T>(default, success: false, message: message);
        }
    }

    public class PaginationMetadata
    {
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public int TotalCount { get; set; }
        public bool HasNextPage { get; set; }
        public bool HasPreviousPage { get; set; }
    }
}