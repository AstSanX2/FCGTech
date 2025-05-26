using FCG.API.Domain.DTO.Bases;
using FCG.API.Domain.DTO.Bases.Interfaces;
using FCG.API.Domain.Interfaces.Repositories;
using FCG.Domain.Entities;
using MongoDB.Bson;
using MongoDB.Driver;

namespace FCG.API.Infraestructure.Repositories
{
    public class BaseRepository<TEntity>(IMongoDatabase database) : IBaseRepository<TEntity> where TEntity : BaseEntity
    {
        protected readonly IMongoCollection<TEntity> _collection = database.GetCollection<TEntity>(typeof(TEntity).Name);

        public virtual async Task<List<ProjectDTO>> GetAllAsync<ProjectDTO>() where ProjectDTO : IProjectable<TEntity, ProjectDTO>, new()
        {
            var projection = new ProjectDTO().ProjectExpression();
            var dtos = await _collection.Find(_ => true).Project(projection).ToListAsync();
            return dtos;
        }

        public virtual async Task<ProjectDTO?> GetByIdAsync<ProjectDTO>(ObjectId id) where ProjectDTO : IProjectable<TEntity, ProjectDTO>, new()
        {
            var filter = Builders<TEntity>.Filter.Eq("_id", id);
            var projection = new ProjectDTO().ProjectExpression();
            var dto = await _collection.Find(filter).Project(projection).FirstOrDefaultAsync();
            return dto;
        }

        public virtual async Task<List<ProjectDTO>> FindAsync<ProjectDTO>(IFilterDTO<TEntity> dto) where ProjectDTO : IProjectable<TEntity, ProjectDTO>, new()
        {
            var filterExpression = dto.GetFilterExpression();
            var projection = new ProjectDTO().ProjectExpression();
            var dtos = await _collection.Find(filterExpression).Project(projection).ToListAsync();
            return dtos;
        }

        public virtual async Task<ProjectDTO> FindOneAsync<ProjectDTO>(IFilterDTO<TEntity> dto) where ProjectDTO : IProjectable<TEntity, ProjectDTO>, new()
        {
            var filterExpression = dto.GetFilterExpression();
            var projection = new ProjectDTO().ProjectExpression();
            return await _collection.Find(filterExpression).Project(projection).FirstOrDefaultAsync();
        }

        public virtual async Task<TEntity> CreateAsync<CreateDTO>(CreateDTO dto) where CreateDTO : BaseCreateDTO<TEntity>
        {
            var entity = dto.ToEntity();

            await _collection.InsertOneAsync(entity);
            return entity;
        }

        public virtual async Task UpdateAsync<UpdateDTO>(ObjectId id, UpdateDTO dto) where UpdateDTO : BaseUpdateDTO<TEntity>
        {
            var filter = Builders<TEntity>.Filter.Eq("_id", id);
            var entity = dto.GetUpdateDefinition();
            await _collection.UpdateOneAsync(filter, entity);
        }

        public virtual async Task DeleteAsync(ObjectId id)
        {
            var filter = Builders<TEntity>.Filter.Eq("_id", id);
            await _collection.DeleteOneAsync(filter);
        }
    }
}
