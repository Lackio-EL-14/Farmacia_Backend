

using Farmacia.Core.Pagination;
using Farmacia.Infrastructure.DTOs;

public interface IVentaDapperRepository
{
    Task<PagedList<VentaDtos>> GetPagedAsync(Farmacia.Core.DTOs.VentaFilterDto filter);
    Task<VentaDtos?> GetByIdAsync(int id);
}
