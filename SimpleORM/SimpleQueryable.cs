using System.Collections;
using System.Linq.Expressions;
using System.Reflection.Metadata;
using SimpleORM.Interfaces;

namespace SimpleORM;

public class SimpleQueryable<TSource> : ISimpleQueryable<TSource>
{
    private readonly object _dataSource;
    
    public SimpleQueryable(Expression queryDescription, object dataSource)
    {
        _dataSource = dataSource;
        QueryDescription = queryDescription;
    }

    public Expression QueryDescription { get; }

    public IEnumerator<TSource> GetEnumerator()
    {
        return Execute<IEnumerator<TSource>>();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public ISimpleQueryable<TSource> CreateNewQueryable(Expression queryDescription)
    {
        return new SimpleQueryable<TSource>(queryDescription, _dataSource);
    }

    public TResult Execute<TResult>()
    {
        var sql = ExpressionToSqlTranslator.Translate(QueryDescription);

        throw new NotImplementedException();
    }
}