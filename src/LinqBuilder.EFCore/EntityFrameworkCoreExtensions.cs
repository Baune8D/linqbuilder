﻿using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace LinqBuilder.EFCore
{
    public static class EntityFrameworkCoreExtensions
    {
        public static async Task<bool> AnyAsync<TEntity>(this IQueryable<TEntity> query, ISpecification<TEntity> specification)
            where TEntity : class
        {
            if (specification == null) throw Exceptions.SpecificationCannotBeNull(nameof(specification));
            return await query.AnyAsync(specification.AsExpression());
        }

        public static async Task<bool> AllAsync<TEntity>(this IQueryable<TEntity> query, ISpecification<TEntity> specification)
            where TEntity : class
        {
            if (specification == null) throw Exceptions.SpecificationCannotBeNull(nameof(specification));
            return await query.AllAsync(specification.AsExpression());
        }
    }
}