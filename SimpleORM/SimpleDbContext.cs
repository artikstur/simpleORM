namespace SimpleORM;

public class SimpleDbContext
{
    private readonly string _connectionString;
    
    public SimpleDbContext(string connectionString)
    {
        _connectionString = connectionString;
    }
}