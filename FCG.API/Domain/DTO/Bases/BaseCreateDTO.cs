using FCG.Domain.Entities;

namespace FCG.API.Domain.DTO.Bases
{
    public abstract class BaseCreateDTO<TEntity> where TEntity : BaseEntity
    {
        public abstract TEntity ToEntity();

    }
}
