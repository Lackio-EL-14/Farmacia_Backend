using Farmacia.Core.Entities;


namespace Farmacia.Core.Interfaces
{
    public interface IProductoRepository : IBaseRepository<Producto>
    {
        Task<IEnumerable<Producto>> GetProductosAsync(int idUser);
    }
}
