using Farmacia.Core.Entities;
using Farmacia.Core.Interfaces;
using Farmacia.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage; 

namespace Farmacia.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly FarmaciaContext _context;

        // Repositorios
        private IBaseRepository<Producto>? _productos;
        private IBaseRepository<Proveedor>? _proveedores;
        private IBaseRepository<Compra>? _compras;
        private IBaseRepository<DetalleCompra>? _detallesCompra;
        private IBaseRepository<Cliente>? _clientes;
        private IBaseRepository<Venta>? _ventas;
        private IBaseRepository<DetalleVenta>? _detallesVenta;
        private IBaseRepository<Factura>? _facturas;
        private IBaseRepository<MovimientoInventario>? _movimientos;
        private IBaseRepository<Caja>? _cajas;
        private IBaseRepository<ArqueoCaja>? _arqueos;
        private IBaseRepository<Usuario>? _usuarios;

        // Variable para controlar la transacción (Debe ser IDbContextTransaction)
        private IDbContextTransaction? _transaction;

        public UnitOfWork(FarmaciaContext context)
        {
            _context = context;
        }

        // Propiedades de Repositorios
        public IBaseRepository<Producto> Productos => _productos ??= new BaseRepository<Producto>(_context);
        public IBaseRepository<Proveedor> Proveedores => _proveedores ??= new BaseRepository<Proveedor>(_context);
        public IBaseRepository<Compra> Compras => _compras ??= new BaseRepository<Compra>(_context);
        public IBaseRepository<DetalleCompra> DetallesCompra => _detallesCompra ??= new BaseRepository<DetalleCompra>(_context);
        public IBaseRepository<Cliente> Clientes => _clientes ??= new BaseRepository<Cliente>(_context);
        public IBaseRepository<Venta> Ventas => _ventas ??= new BaseRepository<Venta>(_context);
        public IBaseRepository<DetalleVenta> DetallesVenta => _detallesVenta ??= new BaseRepository<DetalleVenta>(_context);
        public IBaseRepository<Factura> Facturas => _facturas ??= new BaseRepository<Factura>(_context);
        public IBaseRepository<MovimientoInventario> Movimientos => _movimientos ??= new BaseRepository<MovimientoInventario>(_context);
        public IBaseRepository<Caja> Cajas => _cajas ??= new BaseRepository<Caja>(_context);
        public IBaseRepository<ArqueoCaja> Arqueos => _arqueos ??= new BaseRepository<ArqueoCaja>(_context);
        public IBaseRepository<Usuario> Usuarios => _usuarios ??= new BaseRepository<Usuario>(_context);

        // Métodos de guardado estándar
        public void SaveChanges()
        {
            try
            {
                _context.SaveChanges();
            }
            catch (DbUpdateException ex)
            {
                throw new Exception($"Error al guardar los cambios: {ex.InnerException?.Message ?? ex.Message}");
            }
        }

        public async Task<int> SaveChangesAsync()
        {
            try
            {
                return await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                throw new Exception($"Error al guardar los cambios: {ex.InnerException?.Message ?? ex.Message}");
            }
        }


        public async Task BeginTransaccionAsync()
        {
            if (_transaction == null)
            {
                // Inicia una transacción en la base de datos
                _transaction = await _context.Database.BeginTransactionAsync();
            }
        }

        public async Task CommitAsync()
        {
            try
            {
                // Asegura guardar cualquier cambio pendiente en el contexto antes de confirmar
                await _context.SaveChangesAsync();

                if (_transaction != null)
                {
                    await _transaction.CommitAsync();
                }
            }
            finally
            {
                if (_transaction != null)
                {
                    await _transaction.DisposeAsync();
                    _transaction = null;
                }
            }
        }

        public async Task RollbackAsync()
        {
            if (_transaction != null)
            {
                await _transaction.RollbackAsync();
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        public void Dispose()
        {
            _transaction?.Dispose(); // Liberar transacción si quedó abierta por error
            _context?.Dispose();
        }
    }
}