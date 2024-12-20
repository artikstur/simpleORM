using Npgsql;
using SimpleORM;
using Document = SimpleORM.Models.Document;

await using var connection =
    new NpgsqlConnection("Host=localhost;Port=5555;Username=postgres;Password=123;Database=learning_db");
await connection.OpenAsync();

int id = 10;
FormattableString sql = $"""
                         SELECT id, content FROM documents;
                         """;

foreach (var doc in await connection.QueryAsync<Document>(sql, default))
{
    Console.WriteLine(doc.DocumentId + " " + doc.Content);
}


