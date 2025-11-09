using Farmacia.Core.DTOs;
using Farmacia.Core.Pagination;

namespace Farmacia.Core.Interfaces
{
    public interface IDetalleVentaService
    {
        Task<PagedList<DetalleVentaDto>> GetDetallesAsync(DetalleVentaFilterDto filter);
        Task<List<DetalleVentaDto>> GetByVentaIdAsync(int ventaId);
        Task<DetalleVentaDto> CreateDetalleAsync(DetalleVentaDto dto);
        Task DeleteDetalleAsync(int id);
    }
}
