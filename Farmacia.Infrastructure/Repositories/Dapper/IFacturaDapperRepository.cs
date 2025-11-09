
using Farmacia.Core.DTOs;
using Farmacia.Core.Pagination;
using Farmacia.Infrastructure.DTOs;
using System.Threading.Tasks;

public interface IFacturaDapperRepository
{
    Task<PagedList<FacturaDtos>> GetPagedAsync(FacturaFilterDto filter);
    Task<FacturaDtos?> GetByIdAsync(int id);
}
