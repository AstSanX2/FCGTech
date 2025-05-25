using FCG.API.Domain.DTO.Bases.Interfaces;
using FCG.Domain.Entities;
using System.Linq.Expressions;

namespace FCG.API.Domain.DTO.Bases
{
    public abstract class BaseProjectDTO<TEntity, TProject> : IProjectable<TEntity, TProject>
        where TEntity : BaseEntity
    {
        public abstract Expression<Func<TEntity, TProject>> ProjectExpression();
    }
}
