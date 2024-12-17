using SimpleORM.Attributes;

namespace SimpleORM.Models;

public class Document
{
    [ColumnName("content")]
    public string? Content { get; set; }
    [ColumnName("id")]
    public int DocumentId { get; set; }
}