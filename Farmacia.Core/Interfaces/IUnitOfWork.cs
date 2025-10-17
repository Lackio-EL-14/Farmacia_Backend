using Farmacia.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Farmacia.Core.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        //Asi con cada nueva tabla que se cree en la base de datos se debe crear un nuevo repositorio
        IBaseRepository<Producto> Productos { get; }
        IBaseRepository<Cliente> Clientes { get; }

        Task<int> SaveChangesAsync();
        void SaveChanges();

    }
}
