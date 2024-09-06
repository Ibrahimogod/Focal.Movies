using System.Linq.Expressions;
using Focal.Movies.API.Builders;

namespace Focal.Movies.API.Extensions;

public static class ExpressionExtensions
{
    public static string ToFilterExpression<TModel>(this Expression<Func<TModel, bool>> expression)
    {
        return FilterExpressionBuilder.FromExpression(expression);
    }

    public static string ToSortExpression<TModel, TProperty>(this Expression<Func<TModel, TProperty>> expression, bool ascending)
    {
        return SortExpressionBuilder.FromExpression(expression, ascending);
    }
}