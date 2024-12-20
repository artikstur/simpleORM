using System.Data;

namespace SimpleORM;

public static class DataReaderExtensions
{
    private static bool TryGetOrdinal(this IDataReader reader, string column, out int order)
    {
        order = -1;
        for (var i = 0; i < reader.FieldCount; i++)
        {
            if (reader.GetName(i) != column) continue;
            order = i;
            return true;
        }

        return false;
    }

    public static bool IsValueNull(this IDataReader reader, int order)
    {
        return reader.IsDBNull(order);
    }

    // Теперь если отсутствует одно из полей в запросе,
    // то оно останется просто со своим значением по умолчанию, без ошибок
    public static string? GetString(this IDataReader reader, string column)
    {
        if (reader.TryGetOrdinal(column, out var order) && !reader.IsValueNull(order))
        {
            return reader.GetString(order);
        }

        return null;
    }

    public static int GetInt32(this IDataReader reader, string column)
    {
        if (reader.TryGetOrdinal(column, out var order) && !reader.IsValueNull(order))
        {
            return reader.GetInt32(order);
        }

        return default;
    }
    
    public static T GetValue<T>(this IDataReader reader, string column)
    {
        if (!reader.TryGetOrdinal(column, out var order) || reader.IsValueNull(order))
            return default!;
        
        var type = typeof(T);

        if (type == typeof(string))
            return (T)(object)reader.GetString(order);

        if (type == typeof(int))
            return (T)(object)reader.GetInt32(order);

        if (type == typeof(Guid))
            return (T)(object)reader.GetGuid(order);

        if (type == typeof(DateTime))
            return (T)(object)reader.GetDateTime(order);
            
        throw new NotSupportedException($"Type {type} is not supported.");
    }
}