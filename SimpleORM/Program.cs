using System.Linq.Expressions;
using SimpleORM;
using SimpleORM.Extensions;
using Document = SimpleORM.Models.Document;

// await using var connection =
//     new NpgsqlConnection("Host=localhost;Port=5555;Username=postgres;Password=123;Database=learning_db");
// await connection.OpenAsync();
//
// int id = 10;
// FormattableString sql = $"""
//                          SELECT id, content FROM documents;
//                          """;
//
// foreach (var doc in await connection.QueryAsync<Document>(sql, default))
// {
//     Console.WriteLine(doc.DocumentId + " " + doc.Content);
// }

var provider = new SimpleQueryable<Document>(
    Expression.Constant(new List<Document>().AsQueryable()),
    null
);

var result = provider
    .WhereCustom(d => d.Content == "документ 1")
    .WhereCustom(d => d.Content == "документ 2")
    .WhereCustom(d => d.Content == "документ 3")
    .WhereCustom(d => d.Content == "документ 4")
    .CountCustom();