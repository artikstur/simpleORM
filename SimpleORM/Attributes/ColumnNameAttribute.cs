namespace SimpleORM.Attributes;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class ColumnNameAttribute: Attribute
{
    public string Name { get; }

    public ColumnNameAttribute(string name)
    {
        Name = name;
    }
}