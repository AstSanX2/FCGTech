using FCG.Domain.Entities;
using FCG.Domain.Interfaces.Repositories;
using FCG.Domain.Interfaces.Services;
using MongoDB.Bson;

namespace FCG.Application.Services
{
    public class BaseService<T>(IBaseRepository<T> repo) : IBaseService<T> where T : BaseEntity
    {
        protected readonly IBaseRepository<T> _repo = repo;

        public Task<List<T>> GetAllAsync() => _repo.GetAllAsync();

        public Task<T?> GetByIdAsync(ObjectId id) => _repo.GetByIdAsync(id);

        public Task CreateAsync(T entity) => _repo.CreateAsync(entity);

        public Task UpdateAsync(ObjectId id, T entity)
        {
            entity._id = id;
            return _repo.UpdateAsync(id, entity);
        }

        public Task DeleteAsync(ObjectId id) => _repo.DeleteAsync(id);
    }

}
