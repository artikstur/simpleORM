using System.Linq.Expressions;
using SimpleORM.Interfaces;

namespace SimpleORM.Extensions;

public static class SimpleQueryableExtensions
{
    public static ISimpleQueryable<TSource> WhereCustom<TSource>(
        this ISimpleQueryable<TSource> queryable,
        Expression<Func<TSource, bool>> predicate)
    {
        var currentExpression = ((SimpleQueryable<TSource>)queryable).QueryDescription;
        var newExpression = Expression.Call(
            typeof(Queryable),
            nameof(Queryable.Where), 
            new[] { typeof(TSource) },
            currentExpression, 
            predicate 
        );
        
        return queryable.CreateNewQueryable(newExpression);
    }
  
    public static int CountCustom<TSource>(this ISimpleQueryable<TSource> queryable) 
    {
        var currentExpression = ((SimpleQueryable<TSource>)queryable).QueryDescription;
        var temp = queryable.CreateNewQueryable(currentExpression);
        return temp.Execute<int>();
    }
}