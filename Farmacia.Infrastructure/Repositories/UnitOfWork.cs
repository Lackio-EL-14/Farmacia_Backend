using Farmacia.Core.Entities;
using Farmacia.Core.Interfaces;
using Farmacia.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Farmacia.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly FarmaciaContext _context;

        // Campos privados
        private IBaseRepository<Producto>? _productos;
        private IBaseRepository<Cliente>? _clientes;
        // Aquí puedes añadir los demás repositorios (Proveedores, Ventas, etc.)

        public UnitOfWork(FarmaciaContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        // Repositorios expuestos
        public IBaseRepository<Producto> Productos => _productos ??= new BaseRepository<Producto>(_context);
        public IBaseRepository<Cliente> Clientes => _clientes ??= new BaseRepository<Cliente>(_context);

        // Métodos de guardado
        public void SaveChanges()
        {
            try
            {
                _context.SaveChanges();
            }
            catch (DbUpdateException ex)
            {
                // Captura errores de integridad referencial (FK, duplicados, etc.)
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
                // Mismo control pero en versión async
                throw new Exception($"Error al guardar los cambios: {ex.InnerException?.Message ?? ex.Message}");
            }
        }

        // Liberar contexto
        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}
