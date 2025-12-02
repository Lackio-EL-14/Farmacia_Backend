using Farmacia.Core.Entities;


namespace Farmacia.Core.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {

        IBaseRepository<Producto> Productos { get; }
        IBaseRepository<Proveedor> Proveedores { get; }
        IBaseRepository<Compra> Compras { get; }
        IBaseRepository<DetalleCompra> DetallesCompra { get; }
        IBaseRepository<Cliente> Clientes { get; }
        IBaseRepository<Venta> Ventas { get; }
        IBaseRepository<DetalleVenta> DetallesVenta { get; }
        IBaseRepository<Factura> Facturas { get; }
        IBaseRepository<MovimientoInventario> Movimientos { get; }
        IBaseRepository<Caja> Cajas { get; }
        IBaseRepository<ArqueoCaja> Arqueos { get; }
        IBaseRepository<Usuario> Usuarios { get; }


        Task<int> SaveChangesAsync();
        void SaveChanges();
        Task BeginTransaccionAsync();
        Task CommitAsync();
        Task RollbackAsync();
    }
}
