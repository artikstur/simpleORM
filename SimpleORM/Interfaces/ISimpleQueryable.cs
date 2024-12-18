using System.Linq.Expressions;

namespace SimpleORM.Interfaces;

public interface ISimpleQueryable<T> : IEnumerable<T>
{
    Expression QueryDescription { get; }
    ISimpleQueryable<T> CreateNewQueryable(Expression queryDescription);
    TResult Execute<TResult>();
}