using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Design;

namespace DemoWebApi_BAL.Extension
{
    static class LinqHelper
    {

        //LINQ helper accepts a flag condition (for implementing iFlogic) and lambda expression return Where result
        public static IQueryable<T> WhereIf<T>(this IQueryable<T> query, bool condition,
            Expression<Func<T, bool>> whereClause)
        {
            if (condition)
            {
                return query.Where(whereClause);
            }

            return query;
        }

        //LINQ helper accepts a flag descending and lambda expression return OrderBy result
        public static IQueryable<TSource> OrderByIf<TSource, TKey>(this IQueryable<TSource>source, bool? isDescending,
            Expression<Func<TSource, TKey>> keySelector)
        {
            if (isDescending is null)
            {
                return source;
            }
            if (isDescending is true)
            {
                return source.OrderByDescending(keySelector);
            }
            return source.OrderBy(keySelector);
            

        }
       
    }
    
}
