using Farmacia.Core.DTOs;

using Farmacia.Core.Pagination;


namespace Farmacia.Core.Interfaces
{
    public interface IFacturaService
    {
        Task<PagedList<FacturaDtos>> GetFacturasAsync(FacturaFilterDto filter);
        Task<FacturaDtos?> GetFacturaByIdAsync(int id);
        Task<FacturaDtos> CreateFacturaAsync(FacturaDtos dto);
        Task DeleteFacturaAsync(int id);
    }
}
