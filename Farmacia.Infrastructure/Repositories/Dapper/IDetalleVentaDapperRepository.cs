
using Farmacia.Core.DTOs;
using Farmacia.Core.Pagination;
using Farmacia.Infrastructure.DTOs;
using System.Threading.Tasks;

public interface IDetalleVentaDapperRepository
{
    Task<IEnumerable<DetalleVentaDto>> GetByVentaIdAsync(int ventaId);
    Task<PagedList<DetalleVentaDto>> GetPagedAsync(DetalleVentaFilterDto filter);
}
