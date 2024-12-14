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

    // Теперь если отсутствует одно из полей в запросе,
    // то оно останется просто со своим значением по умолчанию, без ошибок
    public static string? GetString(this IDataReader reader, string column)
    {
        if (reader.TryGetOrdinal(column, out int order))
        {
            return reader.GetString(order);
        }

        return null;
    }

    public static int GetInt32(this IDataReader reader, string column)
    {
        if (reader.TryGetOrdinal(column, out int order))
        {
            return reader.GetInt32(order);
        }

        return default;
    }
}