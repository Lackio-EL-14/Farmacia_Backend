using Farmacia.Core.Entities;


namespace Farmacia.Core.Interfaces
{
    public interface IBaseRepository<T> where T : BaseEntities
    {
        Task<IEnumerable<T>> GetAll();
        Task<T> GetById(int id);
        Task Add(T entity);
        Task Update(T entity);
        Task Delete(int id);
    }
}

