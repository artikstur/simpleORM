using System.Collections.Concurrent;
using System.Data;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.RegularExpressions;
using Npgsql;

namespace SimpleORM;

public static class QueryMapper
{
    private static ConcurrentDictionary<Type, Delegate> _mapperFuncs = new();

    private static readonly MethodInfo
        GetStringMethod = typeof(DataReaderExtensions).GetMethod("GetString")!,
        GetInt32Method = typeof(DataReaderExtensions).GetMethod("GetInt32")!;

    public static async Task<List<T>> QueryAsync<T>(this NpgsqlConnection connection, FormattableString sql,
        CancellationToken cancellationToken)
    {
        await using var command = connection.CreateCommand();
        command.CommandText = ReplaceParameters(sql.Format);
        
        for (int i = 0; i < sql.ArgumentCount; i++)
        {
            command.Parameters.AddWithValue($"@p{i}", sql.GetArgument(i));
        }
        
        await using var reader = await command.ExecuteReaderAsync(cancellationToken);
        var list = new List<T>();
        
        var func = (Func<IDataReader, T>)_mapperFuncs.GetOrAdd(typeof(T), x => Build<T>());
        while (await reader.ReadAsync(cancellationToken))
        {
            list.Add(func(reader));
        }

        return list;
    }

    private static string ReplaceParameters(string query)
    {
        return Regex.Replace(query, @"\{(\d+)\}", x => $"@p{x.Groups[1].Value}"); // {0} -> @p1
    }

    private static Func<IDataReader, T> Build<T>()
    {
        var readerParam = Expression.Parameter(typeof(IDataReader));
        var newExp = Expression.New(typeof(T));

        var memberInit = Expression.MemberInit(newExp,
            typeof(T).GetProperties().Select(x => Expression.Bind(x, BuildReadColumnExpression(readerParam, x))));

        return Expression.Lambda<Func<IDataReader, T>>(memberInit, readerParam).Compile();
    }

    private static Expression BuildReadColumnExpression(Expression reader, PropertyInfo prop)
    {
        if (prop.PropertyType == typeof(string))
        {
            return Expression.Call(null, GetStringMethod, reader, Expression.Constant(prop.Name.ToLower()));
        }

        if (prop.PropertyType == typeof(int))
        {
            return Expression.Call(null, GetInt32Method, reader, Expression.Constant(prop.Name.ToLower()));
        }

        throw new InvalidOperationException();
    }
}