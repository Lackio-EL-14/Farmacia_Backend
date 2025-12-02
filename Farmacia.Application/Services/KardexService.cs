using Farmacia.Core.Entities;
using Farmacia.Core.Interfaces;
using Farmacia.Core.Pagination;

namespace Farmacia.Application.Services
{
    public class KardexService
    {
        private readonly IUnitOfWork _unitOfWork;

        public KardexService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<PagedList<MovimientoInventario>> GetKardexByProducto(int productoId, int pageNumber, int pageSize)
        {

            var todosLosMovimientos = await _unitOfWork.Movimientos.GetAll();

            var historialProducto = todosLosMovimientos
                .Where(x => x.ProductoId == productoId)
                .OrderByDescending(x => x.Fecha) 
                .ToList();

            return PagedList<MovimientoInventario>.Create(historialProducto, pageNumber, pageSize);
        }
    }
}