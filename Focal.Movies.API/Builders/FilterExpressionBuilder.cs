using System.Linq.Expressions;

namespace Focal.Movies.API.Builders;

public static class FilterExpressionBuilder
{
    public static string FromExpression<TModel>(Expression<Func<TModel, bool>> expression)
    {
        if (expression.Body is BinaryExpression binaryExpression)
        {
            return ProcessBinaryExpression(binaryExpression);
        }

        throw new NotSupportedException("Only binary expressions are supported in this example.");
    }

    private static string ProcessBinaryExpression(BinaryExpression binaryExpression)
    {
        var left = ProcessExpression(binaryExpression.Left);
        var right = ProcessExpression(GetConstantExpression(binaryExpression.Right));
        var @operator = GetOperator(binaryExpression.NodeType);

        return $"({left} {@operator} {right})";
    }

    private static ConstantExpression GetConstantExpression(Expression expression)
    {
        if (expression is MemberExpression { Expression: ConstantExpression } memberExpression)
        {
            var lambda = Expression.Lambda<Func<object>>(Expression.Convert(memberExpression, typeof(object)));

            var compiledLambda = lambda.Compile();

            var value = compiledLambda();
            return Expression.Constant(value);
        }
        
        throw new NotSupportedException("Unsupported member type.");
    }

    private static string ProcessExpression(Expression expression)
    {
        switch (expression)
        {
            case MemberExpression memberExpression:
                return ProcessMemberExpression(memberExpression);
            case ConstantExpression constantExpression:
                return ProcessConstantExpression(constantExpression);
            case UnaryExpression unaryExpression:
                return ProcessUnaryExpression(unaryExpression);
            default:
                throw new NotSupportedException($"Expression type '{expression.GetType()}' is not supported.");
        }
    }

    private static string ProcessMemberExpression(MemberExpression memberExpression)
    {
        return memberExpression.Member.Name;
    }

    private static string ProcessConstantExpression(ConstantExpression constantExpression)
    {
        if (constantExpression.Value is string)
        {
            return $"'{constantExpression.Value}'";
        }

        return constantExpression.Value.ToString();
    }

    private static string ProcessUnaryExpression(UnaryExpression unaryExpression)
    {
        if (unaryExpression.Operand is ConstantExpression constantExpression)
        {
            var value = ProcessConstantExpression(constantExpression);
            return unaryExpression.NodeType == ExpressionType.Negate ? $"-{value}" : value;
        }

        throw new NotSupportedException($"Unary expression type '{unaryExpression.NodeType}' is not supported.");
    }

    private static string GetOperator(ExpressionType nodeType)
    {
        return nodeType switch
        {
            ExpressionType.Equal => "=",
            ExpressionType.NotEqual => "<>",
            ExpressionType.GreaterThan => ">",
            ExpressionType.GreaterThanOrEqual => ">=",
            ExpressionType.LessThan => "<",
            ExpressionType.LessThanOrEqual => "<=",
            _ => throw new NotSupportedException($"Operator '{nodeType}' is not supported.")
        };
    }
}