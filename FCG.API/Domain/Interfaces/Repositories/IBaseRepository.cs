using FCG.API.Domain.DTO.Bases;
using FCG.API.Domain.DTO.Bases.Interfaces;
using FCG.Domain.Entities;
using MongoDB.Bson;

namespace FCG.API.Domain.Interfaces.Repositories
{
    public interface IBaseRepository<TEntity> where TEntity : BaseEntity
    {
        Task<TEntity> CreateAsync<CreateDTO>(CreateDTO dto) where CreateDTO : BaseCreateDTO<TEntity>;
        Task DeleteAsync(ObjectId id);
        Task<List<ProjectDTO>> FindAsync<ProjectDTO>(IFilterDTO<TEntity> dto) where ProjectDTO : IProjectable<TEntity, ProjectDTO>, new();
        Task<ProjectDTO> FindOneAsync<ProjectDTO>(IFilterDTO<TEntity> dto) where ProjectDTO : IProjectable<TEntity, ProjectDTO>, new();
        Task<List<ProjectDTO>> GetAllAsync<ProjectDTO>() where ProjectDTO : IProjectable<TEntity, ProjectDTO>, new();
        Task<ProjectDTO> GetByIdAsync<ProjectDTO>(ObjectId id) where ProjectDTO : IProjectable<TEntity, ProjectDTO>, new();
        Task UpdateAsync<UpdateDTO>(ObjectId id, UpdateDTO dto) where UpdateDTO : BaseUpdateDTO<TEntity>;
    }
}