using FCG.API.Domain.DTO.Bases.Interfaces;
using FCG.Domain.Entities;
using MongoDB.Bson;
using System.Linq.Expressions;

namespace FCG.API.Domain.DTO.Bases
{
    public abstract class BaseFilterDTO<TEntity> : IFilterDTO<TEntity> where TEntity : BaseEntity
    {
        public virtual Expression<Func<TEntity, bool>> GetFilterExpression(ObjectId id)
        {
            return x => x._id == id;
        }

        public abstract Expression<Func<TEntity, bool>> GetFilterExpression();
    }
}
