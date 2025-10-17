using Farmacia.Core.Entities;
using Farmacia.Core.Interfaces;
using Farmacia.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Farmacia.Infrastructure.Repositories
{
    public class BaseRepository<T> : IBaseRepository<T> where T : BaseEntities
    {
        private readonly FarmaciaContext _context;
        protected readonly DbSet<T> _entities;

        public BaseRepository(FarmaciaContext context)
        {
            _context = context;
            _entities = context.Set<T>();
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            return await _entities.ToListAsync();
        }

        public async Task<T?> GetById(int id)
        {
            return await _entities.FindAsync(id);
        }

        public async Task Add(T entity)
        {
            _entities.Add(entity);
            
        }

        public async Task Update(T entity)
        {
            _entities.Update(entity);
            
        }

        public async Task Delete(int id)
        {
            T? entity = await GetById(id);
            if (entity != null)
            {
                _entities.Remove(entity);
                
            }
        }
    }
}
