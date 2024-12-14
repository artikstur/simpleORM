using System.Reflection.Metadata;
using Npgsql;
using SimpleORM;
using Document = SimpleORM.Models.Document;

await using var connection =
    new NpgsqlConnection("Host=localhost;Port=5555;Username=postgres;Password=postgres;Database=learning_db");
await connection.OpenAsync();

int id = 10;
FormattableString sql = $"""
                   SELECT id, content FROM documents WHERE id > {id};
                   """;

foreach (var doc in await connection.QueryAsync<Document>(sql, default))
{
    Console.WriteLine(doc.Id + " " + doc.Content);
}