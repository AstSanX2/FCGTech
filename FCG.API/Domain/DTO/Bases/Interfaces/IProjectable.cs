﻿using FCG.Domain.Entities;
using System.Linq.Expressions;

namespace FCG.API.Domain.DTO.Bases.Interfaces
{
    public interface IProjectable<TEntity, TDTO> where TEntity : BaseEntity
    {
        Expression<Func<TEntity, TDTO>> ProjectExpression();
    }
}
