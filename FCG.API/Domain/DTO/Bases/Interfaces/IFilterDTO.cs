using FCG.Domain.Entities;
using MongoDB.Bson;
using System.Linq.Expressions;

namespace FCG.API.Domain.DTO.Bases.Interfaces
{
    public interface IFilterDTO<TEntity> where TEntity : BaseEntity
    {
        Expression<Func<TEntity, bool>> GetFilterExpression();
    }
}
