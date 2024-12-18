namespace SimpleORM;
using System.Linq.Expressions;
using System.Text;

public class ExpressionToSqlTranslator : ExpressionVisitor
{
    private readonly StringBuilder _sql = new();
    private bool _isFirstCondition = true;  

    public static string Translate(Expression expression)
    {
        var visitor = new ExpressionToSqlTranslator();
        visitor.Visit(expression);
        return visitor._sql.ToString();
    }
    
    private static string TableName = "Document";

    public ExpressionToSqlTranslator()
    {
        _sql.Append($"SELECT * FROM {TableName} WHERE ");
    }
    
    protected override Expression VisitBinary(BinaryExpression node)
    {
        if (!_isFirstCondition)
        {
            _sql.Append(" AND "); 
        }
        _isFirstCondition = false;

        _sql.Append("(");
        Visit(node.Left); 
        _sql.Append(GetSqlOperator(node.NodeType)); 
        Visit(node.Right); 
        _sql.Append(")");
        return node;
    }
    
    protected override Expression VisitMember(MemberExpression node)
    {
        if (node.Expression is ConstantExpression constant && constant.Value is IQueryable)
        {
            return node;
        }

        _sql.Append(node.Member.Name);
        return node;
    }
    
    protected override Expression VisitConstant(ConstantExpression node)
    {
        if (node.Value is IQueryable)
        {
            return node;
        }

        _sql.Append($"'{node.Value}'");
        return node;
    }
    
    private static string GetSqlOperator(ExpressionType nodeType)
        => nodeType switch
        {
            ExpressionType.Equal => " = ",
            ExpressionType.AndAlso => " AND ", 
            ExpressionType.OrElse => " OR ",
            ExpressionType.GreaterThan => " > ",
            ExpressionType.LessThan => " < ",
            _ => throw new NotSupportedException($"Оператор {nodeType} не поддерживается")
        };
}