using Farmacia.Core.Entities;


namespace Farmacia.Core.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {

        IBaseRepository<Producto> Productos { get; }
        IBaseRepository<Cliente> Clientes { get; }
        IBaseRepository<Venta> Ventas { get; }
        IBaseRepository<DetalleVenta> DetallesVenta { get; }
        IBaseRepository<Factura> Facturas { get; }


        Task<int> SaveChangesAsync();
        void SaveChanges();

    }
}
