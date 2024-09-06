using System.Linq.Expressions;

namespace Focal.Movies.API.Builders;

public static class SortExpressionBuilder
{
    public static string FromExpression<TModel, TProperty>(Expression<Func<TModel, TProperty>> expression, bool ascending)
    {
        if (expression.Body is MemberExpression memberExpression)
        {
            var columnName = ProcessMemberExpression(memberExpression);
            return ascending ? columnName : $"{columnName} DESC";
        }
        
        throw new NotSupportedException("Only member expressions are supported in this example.");
    }

    private static string ProcessMemberExpression(MemberExpression memberExpression)
    {
        return memberExpression.Member.Name;
    }
}