using Farmacia.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Farmacia.Core.Interfaces
{
    public interface IProductoRepository : IBaseRepository<Producto>
    {
        Task<IEnumerable<Producto>> GetProductosAsync(int idUser);
    }
}
