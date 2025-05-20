using FCG.Domain.Entities;
using MongoDB.Bson;

namespace FCG.Domain.Interfaces.Services
{

    public interface IBaseService<T> where T : BaseEntity
    {
        Task<List<T>> GetAllAsync();
        Task<T?> GetByIdAsync(ObjectId id);
        Task CreateAsync(T entity);
        Task UpdateAsync(ObjectId id, T entity);
        Task DeleteAsync(ObjectId id);
    }

}
