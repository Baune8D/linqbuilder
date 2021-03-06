﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace LinqBuilder.OrderBy
{
    public static class LinqExtensions
    {
        public static IOrderedQueryable<TEntity> OrderBy<TEntity>(this IQueryable<TEntity> query, IOrderSpecification<TEntity> orderSpecification)
            where TEntity : class
        {
            if (orderSpecification == null)
            {
                throw new ArgumentNullException(nameof(orderSpecification));
            }

            return orderSpecification.InvokeSort(query);
        }

        public static IOrderedEnumerable<TEntity> OrderBy<TEntity>(this IEnumerable<TEntity> collection, IOrderSpecification<TEntity> orderSpecification)
            where TEntity : class
        {
            if (orderSpecification == null)
            {
                throw new ArgumentNullException(nameof(orderSpecification));
            }

            return orderSpecification.InvokeSort(collection);
        }

        public static IOrderedQueryable<TEntity> ThenBy<TEntity>(this IOrderedQueryable<TEntity> query, IOrderSpecification<TEntity> orderSpecification)
            where TEntity : class
        {
            if (orderSpecification == null)
            {
                throw new ArgumentNullException(nameof(orderSpecification));
            }

            return orderSpecification.InvokeSort(query);
        }

        public static IOrderedEnumerable<TEntity> ThenBy<TEntity>(this IOrderedEnumerable<TEntity> collection, IOrderSpecification<TEntity> orderSpecification)
            where TEntity : class
        {
            if (orderSpecification == null)
            {
                throw new ArgumentNullException(nameof(orderSpecification));
            }

            return orderSpecification.InvokeSort(collection);
        }
    }
}
