using Farmacia.Core.Entities;
using Farmacia.Core.Interfaces;
using Farmacia.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Farmacia.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly FarmaciaContext _context;
        private IBaseRepository<Producto>? _productos;
        private IBaseRepository<Cliente>? _clientes;
        private IBaseRepository<Venta>? _ventas;
        private IBaseRepository<DetalleVenta>? _detallesVenta;
        private IBaseRepository<Factura>? _facturas;

        public UnitOfWork(FarmaciaContext context)
        {
            _context = context;
        }

        public IBaseRepository<Producto> Productos => _productos ??= new BaseRepository<Producto>(_context);
        public IBaseRepository<Cliente> Clientes => _clientes ??= new BaseRepository<Cliente>(_context);
        public IBaseRepository<Venta> Ventas => _ventas ??= new BaseRepository<Venta>(_context);
        public IBaseRepository<DetalleVenta> DetallesVenta => _detallesVenta ??= new BaseRepository<DetalleVenta>(_context);
        public IBaseRepository<Factura> Facturas => _facturas ??= new BaseRepository<Factura>(_context);


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

        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}
