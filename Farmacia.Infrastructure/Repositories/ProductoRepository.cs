using Farmacia.Core.Entities;
using Farmacia.Core.Interfaces;
using Farmacia.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
/*

namespace Farmacia.Infrastructure.Repositories
{
    public class ProductoRepository : BaseRepository<Producto>, IProductoRepository
    {
        //private readonly FarmaciaContext _context; No debemos acceder a la base de datos desde el repositorio si no desde el UnitOfWork
        public ProductoRepository(FarmaciaContext context) : base(context)
        {
           // _context = context;
        }

        public async Task<IEnumerable<Producto>> GetProductosAsync(int idUser)
        {
            var productos = await _entities
                .Where(p => p.UserId == idUser)
                .ToListAsync();
            return productos;
        }
    }
}
*/