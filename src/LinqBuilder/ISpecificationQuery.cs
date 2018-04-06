﻿using System.Collections.Generic;
using System.Linq;

namespace LinqBuilder
{
    public interface ISpecificationQuery<TEntity>
    {
        IQueryable<TEntity> Invoke(IQueryable<TEntity> query);
        IEnumerable<TEntity> Invoke(IEnumerable<TEntity> collection);
    }
}