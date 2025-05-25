using FCG.Domain.Entities;
using MongoDB.Driver;

namespace FCG.API.Domain.DTO.Bases
{
    public abstract class BaseUpdateDTO<TEntity> where TEntity : BaseEntity
    {
        public abstract UpdateDefinition<TEntity> GetUpdateDefinition();

    }
}
