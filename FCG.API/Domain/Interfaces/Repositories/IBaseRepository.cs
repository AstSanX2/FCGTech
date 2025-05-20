using FCG.Domain.Entities;
using MongoDB.Bson;
using System.Linq.Expressions;

namespace FCG.Domain.Interfaces.Repositories
{
    public interface IBaseRepository<T> where T : BaseEntity
    {
        Task CreateAsync(T entity);
        Task DeleteAsync(ObjectId id);
        Task<List<T>> FindAsync(Expression<Func<T, bool>> filter);
        Task<List<T>> GetAllAsync();
        Task<T?> GetByIdAsync(ObjectId id);
        Task UpdateAsync(ObjectId id, T entity);
    }
}